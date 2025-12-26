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
    // для настройки повторных попыток - ретраи
    private readonly int _connectRetryAttempts = Math.Max(1, settings.Value.ConnectRetryAttempts);

    private readonly TimeSpan _connectRetryDelay =
        TimeSpan.FromMilliseconds(Math.Max(0, settings.Value.ConnectRetryDelayMs));

    private readonly int _publishRetryAttempts = Math.Max(1, settings.Value.PublishRetryAttempts);

    private readonly TimeSpan _publishRetryDelay =
        TimeSpan.FromMilliseconds(Math.Max(0, settings.Value.PublishRetryDelayMs));

    private readonly double _retryBackoffFactor =
        settings.Value.RetryBackoffFactor <= 0 ? 2 : settings.Value.RetryBackoffFactor;

    private readonly string _streamName = GetRequired(settings.Value.StreamName, "StreamName");
    private readonly string _subjectName = GetRequired(settings.Value.SubjectName, "SubjectName");


    public async Task SendAsync(IList<LeaseCreateUpdateDto> batch, CancellationToken cancellationToken)
    {
        if (batch.Count == 0)
        {
            logger.LogInformation("Skipping empty lease batch.");
            return;
        }

        try
        {
            // await connection.ConnectAsync();
            // вызов с повторными попытками
            await ExecuteWithRetryAsync(
                "connect to NATS",
                _connectRetryAttempts, // сколько раз пытаться
                _connectRetryDelay, // начальная задержка
                async () => await connection.ConnectAsync(),
                cancellationToken);

            INatsJSContext context = connection.CreateJetStreamContext();

            var streamConfig = new StreamConfig(_streamName, [_subjectName]);


            // await context.CreateOrUpdateStreamAsync(streamConfig, cancellationToken);
            await ExecuteWithRetryAsync(
                "create/update stream",
                _publishRetryAttempts,
                _publishRetryDelay,
                async () => await context.CreateOrUpdateStreamAsync(streamConfig, cancellationToken),
                cancellationToken);

            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            // await context.PublishAsync(subject: _subjectName, data: payload, cancellationToken: cancellationToken);
            await ExecuteWithRetryAsync(
                "publish batch",
                _publishRetryAttempts,
                _publishRetryDelay,
                async () => await context.PublishAsync(
                    _subjectName,
                    payload,
                    cancellationToken: cancellationToken),
                cancellationToken);

            logger.LogInformation(
                "Sent a batch of {count} leases to {subject} of {stream}",
                batch.Count, _subjectName, _streamName);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Exception occurred during sending a batch of {count} leases to {stream}/{subject}",
                batch.Count, _streamName, _subjectName);
        }
    }

    // механизм повторных попыток
    private async Task ExecuteWithRetryAsync(
        string operation,
        int attempts, // Максимальное количество попыток
        TimeSpan baseDelay, // Начальная задержка
        Func<Task> action, // Операция для выполнения
        CancellationToken cancellationToken) // Токен отмены
    {
        TimeSpan delay = baseDelay;
        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                await action();
                return;
            }
            catch (Exception ex) when (attempt < attempts && !cancellationToken.IsCancellationRequested)
            {
                // есть еще попытки, операцию не отменили
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

                    // увеличить задержку
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

    private static string GetRequired(string? value, string key) //мини проверка конфигов
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new KeyNotFoundException($"{key} is not configured in Nats section.");
        }

        return value;
    }
}