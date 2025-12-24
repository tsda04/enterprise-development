using Microsoft.Extensions.Options;
using BikeRental.Generator.Nats.Host.Generator;

namespace BikeRental.Generator.Nats.Host;
public sealed class LeaseBatchWorker(
    LeaseBatchGenerator generator,
    BikeRentalNatsProducer producer,
    IOptions<LeaseGenerationOptions> options,
    ILogger<LeaseBatchWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = options.Value;
        if (settings.BatchSize <= 0)
        {
            logger.LogError("LeaseGeneration.BatchSize must be greater than 0.");
            return;
        }

        if (settings.IntervalSeconds <= 0)
        {
            await SendBatchAsync(settings, stoppingToken);
            return;
        }

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(settings.IntervalSeconds));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await SendBatchAsync(settings, stoppingToken);
        }
    }

    private async Task SendBatchAsync(LeaseGenerationOptions settings, CancellationToken stoppingToken)
    {
        var batch = generator.GenerateBatch(settings);
        await producer.SendAsync(batch, stoppingToken);
    }
}

