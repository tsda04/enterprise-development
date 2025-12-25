using System.Text.Json;
using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Infrastructure.Database;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;

namespace BikeRental.Api.Messaging;

internal sealed class NatsLeaseConsumer(
    INatsConnection connection,
    IOptions<NatsConsumerSettings> settings,
    IServiceScopeFactory scopeFactory,
    ILogger<NatsLeaseConsumer> logger) : BackgroundService
{
    private readonly NatsConsumerSettings _settings = settings.Value;
    private readonly INatsDeserialize<byte[]> _deserializer = BuildDeserializer(connection);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ValidateSettings(_settings);

        await ExecuteWithRetryAsync(
            "connect to NATS",
            _settings.ConnectRetryAttempts,
            TimeSpan.FromMilliseconds(_settings.ConnectRetryDelayMs),
            stoppingToken,
            async () => await connection.ConnectAsync());

        var context = connection.CreateJetStreamContext();
        var streamConfig = new StreamConfig(_settings.StreamName, new List<string> { _settings.SubjectName });
        await ExecuteWithRetryAsync(
            "create/update stream",
            _settings.ConnectRetryAttempts,
            TimeSpan.FromMilliseconds(_settings.ConnectRetryDelayMs),
            stoppingToken,
            async () => await context.CreateOrUpdateStreamAsync(streamConfig, stoppingToken));

        var consumerConfig = new ConsumerConfig
        {
            Name = _settings.DurableName,
            DurableName = _settings.DurableName,
            AckPolicy = ConsumerConfigAckPolicy.Explicit,
            DeliverPolicy = ConsumerConfigDeliverPolicy.All,
            ReplayPolicy = ConsumerConfigReplayPolicy.Instant,
            FilterSubject = _settings.SubjectName,
            AckWait = TimeSpan.FromSeconds(Math.Max(1, _settings.AckWaitSeconds)),
            MaxDeliver = Math.Max(1, _settings.MaxDeliver)
        };

        var consumer = await ExecuteWithRetryAsync(
            "create/update consumer",
            _settings.ConnectRetryAttempts,
            TimeSpan.FromMilliseconds(_settings.ConnectRetryDelayMs),
            stoppingToken,
            async () => await context.CreateOrUpdateConsumerAsync(_settings.StreamName, consumerConfig, stoppingToken));

        var consumeOptions = new NatsJSConsumeOpts
        {
            MaxMsgs = Math.Max(1, _settings.ConsumeMaxMsgs),
            Expires = TimeSpan.FromSeconds(Math.Max(1, _settings.ConsumeExpiresSeconds))
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await foreach (var msg in consumer.ConsumeAsync(_deserializer, consumeOptions, stoppingToken))
                {
                    await HandleMessageAsync(msg, stoppingToken);
                }
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Error while consuming leases from NATS. Retrying in {delay}ms.", _settings.ConsumeRetryDelayMs);
                await Task.Delay(Math.Max(0, _settings.ConsumeRetryDelayMs), stoppingToken);
            }
        }
    }

    private async Task HandleMessageAsync(INatsJSMsg<byte[]> msg, CancellationToken stoppingToken)
    {
        if (msg.Data is null || msg.Data.Length == 0)
        {
            logger.LogWarning("Received empty lease batch message.");
            await msg.AckAsync(cancellationToken: stoppingToken);
            return;
        }

        List<LeaseCreateUpdateDto>? leases;
        try
        {
            leases = JsonSerializer.Deserialize<List<LeaseCreateUpdateDto>>(msg.Data);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize lease batch payload.");
            await msg.AckTerminateAsync(cancellationToken: stoppingToken);
            return;
        }

        if (leases is null || leases.Count == 0)
        {
            logger.LogWarning("Received lease batch with no items.");
            await msg.AckAsync(cancellationToken: stoppingToken);
            return;
        }

        try
        {
            await SaveBatchAsync(leases, stoppingToken);
            await msg.AckAsync(cancellationToken: stoppingToken);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Lease batch contains invalid references. Message will be terminated.");
            await msg.AckTerminateAsync(cancellationToken: stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to persist lease batch. Message will be retried.");
        }
    }

    private async Task SaveBatchAsync(IReadOnlyList<LeaseCreateUpdateDto> leases, CancellationToken stoppingToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var leaseService = scope.ServiceProvider.GetRequiredService<ILeaseService>();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);
        foreach (var lease in leases)
        {
            await leaseService.Create(lease);
        }
        await transaction.CommitAsync(stoppingToken);
    }

    private async Task ExecuteWithRetryAsync(
        string operation,
        int attempts,
        TimeSpan baseDelay,
        CancellationToken stoppingToken,
        Func<Task> action)
    {
        _ = await ExecuteWithRetryAsync(
            operation,
            attempts,
            baseDelay,
            stoppingToken,
            async () =>
            {
                await action();
                return new object();
            });
    }

    private async Task<T> ExecuteWithRetryAsync<T>(
        string operation,
        int attempts,
        TimeSpan baseDelay,
        CancellationToken stoppingToken,
        Func<Task<T>> action)
    {
        var retries = Math.Max(1, attempts);
        var delay = baseDelay;
        var backoff = _settings.RetryBackoffFactor <= 0 ? 2 : _settings.RetryBackoffFactor;

        for (var attempt = 1; attempt <= retries; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (attempt < retries && !stoppingToken.IsCancellationRequested)
            {
                if (delay > TimeSpan.Zero)
                {
                    logger.LogWarning(
                        ex,
                        "Failed to {operation} (attempt {attempt}/{retries}). Retrying in {delay}.",
                        operation,
                        attempt,
                        retries,
                        delay);
                    await Task.Delay(delay, stoppingToken);
                    delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * backoff);
                }
                else
                {
                    logger.LogWarning(
                        ex,
                        "Failed to {operation} (attempt {attempt}/{retries}). Retrying immediately.",
                        operation,
                        attempt,
                        retries);
                }
            }
        }

        throw new InvalidOperationException($"Failed to {operation} after {retries} attempts.");
    }

    private static INatsDeserialize<byte[]> BuildDeserializer(INatsConnection connection)
    {
        var registry = connection.Opts.SerializerRegistry;
        return registry.GetDeserializer<byte[]>();
    }

    private static void ValidateSettings(NatsConsumerSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.StreamName))
        {
            throw new KeyNotFoundException("StreamName is not configured in Nats section.");
        }

        if (string.IsNullOrWhiteSpace(settings.SubjectName))
        {
            throw new KeyNotFoundException("SubjectName is not configured in Nats section.");
        }
    }
}
