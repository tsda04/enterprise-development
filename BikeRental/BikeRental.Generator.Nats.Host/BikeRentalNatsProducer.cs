using System.Text.Json;
using BikeRental.Application.Contracts.Dtos;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;

namespace BikeRental.Generator.Nats.Host;

public sealed class BikeRentalNatsProducer(
    IOptions<NatsSettings> settings,
    INatsConnection connection,
    ILogger<BikeRentalNatsProducer> logger)
{
    private readonly string _streamName = GetRequired(settings.Value.StreamName, "StreamName");
    private readonly string _subjectName = GetRequired(settings.Value.SubjectName, "SubjectName");
    private readonly INatsSerialize<byte[]> _serializer = BuildSerializer(connection);
    private readonly NatsJSPubOpts _publishOptions = new();
    private readonly int _connectRetryAttempts = Math.Max(1, settings.Value.ConnectRetryAttempts);
    private readonly TimeSpan _connectRetryDelay = TimeSpan.FromMilliseconds(Math.Max(0, settings.Value.ConnectRetryDelayMs));
    private readonly int _publishRetryAttempts = Math.Max(1, settings.Value.PublishRetryAttempts);
    private readonly TimeSpan _publishRetryDelay = TimeSpan.FromMilliseconds(Math.Max(0, settings.Value.PublishRetryDelayMs));
    private readonly double _retryBackoffFactor = settings.Value.RetryBackoffFactor <= 0 ? 2 : settings.Value.RetryBackoffFactor;

    public async Task SendAsync(IList<LeaseCreateUpdateDto> batch, CancellationToken cancellationToken)
    {
        if (batch.Count == 0)
        {
            logger.LogInformation("Skipping empty lease batch.");
            return;
        }

        try
        {
            await ExecuteWithRetryAsync(
                "connect to NATS",
                _connectRetryAttempts,
                _connectRetryDelay,
                cancellationToken,
                async () => await connection.ConnectAsync());

            var context = connection.CreateJetStreamContext();

            var streamConfig = new StreamConfig(_streamName, new List<string> { _subjectName });
            await ExecuteWithRetryAsync(
                "create/update stream",
                _publishRetryAttempts,
                _publishRetryDelay,
                cancellationToken,
                async () => await context.CreateOrUpdateStreamAsync(streamConfig, cancellationToken));

            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);
            await ExecuteWithRetryAsync(
                "publish batch",
                _publishRetryAttempts,
                _publishRetryDelay,
                cancellationToken,
                async () => await context.PublishAsync(
                    _subjectName,
                    payload,
                    _serializer,
                    _publishOptions,
                    new NatsHeaders(),
                    cancellationToken));

            logger.LogInformation("Sent a batch of {count} leases to {subject} of {stream}", batch.Count, _subjectName, _streamName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} leases to {stream}/{subject}", batch.Count, _streamName, _subjectName);
        }
    }

    private async Task ExecuteWithRetryAsync(
        string operation,
        int attempts,
        TimeSpan baseDelay,
        CancellationToken cancellationToken,
        Func<Task> action)
    {
        var delay = baseDelay;
        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                await action();
                return;
            }
            catch (Exception ex) when (attempt < attempts && !cancellationToken.IsCancellationRequested)
            {
                if (delay > TimeSpan.Zero)
                {
                    logger.LogWarning(
                        ex,
                        "Failed to {operation} (attempt {attempt}/{attempts}). Retrying in {delay}.",
                        operation,
                        attempt,
                        attempts,
                        delay);
                    await Task.Delay(delay, cancellationToken);
                    delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * _retryBackoffFactor);
                }
                else
                {
                    logger.LogWarning(
                        ex,
                        "Failed to {operation} (attempt {attempt}/{attempts}). Retrying immediately.",
                        operation,
                        attempt,
                        attempts);
                }
            }
        }
    }

    private static string GetRequired(string? value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new KeyNotFoundException($"{key} is not configured in Nats section.");
        }
        return value;
    }

    private static INatsSerialize<byte[]> BuildSerializer(INatsConnection connection)
    {
        var registry = connection.Opts.SerializerRegistry ?? new NatsDefaultSerializerRegistry();
        return registry.GetSerializer<byte[]>();
    }
}
