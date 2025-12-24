using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Generator.Nats.Host;

public sealed class LeaseBatchGenerator
{
    public IList<LeaseCreateUpdateDto> GenerateBatch(LeaseGenerationOptions settings)
    {
        Validate(settings);

        var batch = new List<LeaseCreateUpdateDto>(settings.BatchSize);
        for (var i = 0; i < settings.BatchSize; i++)
        {
            var renterId = Random.Shared.Next(settings.RenterIdMin, settings.RenterIdMax + 1);
            var bikeId = Random.Shared.Next(settings.BikeIdMin, settings.BikeIdMax + 1);
            var duration = Random.Shared.Next(settings.RentalDurationMinHours, settings.RentalDurationMaxHours + 1);
            var daysBack = Random.Shared.Next(0, settings.RentalStartDaysBackMax + 1);
            var hoursBack = Random.Shared.Next(0, 24);

            batch.Add(new LeaseCreateUpdateDto
            {
                RenterId = renterId,
                BikeId = bikeId,
                RentalDuration = duration,
                RentalStartTime = DateTime.UtcNow.AddDays(-daysBack).AddHours(-hoursBack)
            });
        }

        return batch;
    }

    private static void Validate(LeaseGenerationOptions settings)
    {
        if (settings.BikeIdMin > settings.BikeIdMax)
        {
            throw new InvalidOperationException("LeaseGeneration.BikeIdMin must be <= LeaseGeneration.BikeIdMax.");
        }

        if (settings.RenterIdMin > settings.RenterIdMax)
        {
            throw new InvalidOperationException("LeaseGeneration.RenterIdMin must be <= LeaseGeneration.RenterIdMax.");
        }

        if (settings.RentalDurationMinHours > settings.RentalDurationMaxHours)
        {
            throw new InvalidOperationException("LeaseGeneration.RentalDurationMinHours must be <= LeaseGeneration.RentalDurationMaxHours.");
        }

        if (settings.RentalStartDaysBackMax < 0)
        {
            throw new InvalidOperationException("LeaseGeneration.RentalStartDaysBackMax must be >= 0.");
        }
    }
}
