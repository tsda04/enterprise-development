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
    
    public async Task SendAsync(IList<LeaseCreateUpdateDto> batch, CancellationToken cancellationToken)
    {
        if (batch.Count == 0)
        {
            logger.LogInformation("Skipping empty lease batch.");
            return;
        }

        try
        {
            await connection.ConnectAsync();
            var context = connection.CreateJetStreamContext();

            var streamConfig = new StreamConfig(_streamName, new List<string> { _subjectName });
            await context.CreateOrUpdateStreamAsync(streamConfig, cancellationToken);

            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);
            await context.PublishAsync(
                subject: _subjectName,
                data: payload,
                cancellationToken: cancellationToken);

            logger.LogInformation("Sent a batch of {count} leases to {subject} of {stream}", batch.Count, _subjectName, _streamName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} leases to {stream}/{subject}", batch.Count, _streamName, _subjectName);
        }
    }

    private static string GetRequired(string? value, string key) // мини проверка
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new KeyNotFoundException($"{key} is not configured in Nats section.");
        }
        return value;
    }
}