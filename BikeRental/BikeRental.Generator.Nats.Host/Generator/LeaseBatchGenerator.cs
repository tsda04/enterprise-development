using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Generator.Nats.Host.Generator;

public sealed class LeaseBatchGenerator
{
    public IList<LeaseCreateUpdateDto> GenerateBatch(LeaseGenerationOptions settings)
    {
        Validate(settings);

        var batch = new List<LeaseCreateUpdateDto>(settings.BatchSize);
        for (var i = 0; i < settings.BatchSize; i++)
        {
            var renterId = PickId(settings.RenterIds, settings.RenterIdMin, settings.RenterIdMax);
            var bikeId = PickId(settings.BikeIds, settings.BikeIdMin, settings.BikeIdMax);
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
        if (settings.BikeIds.Count == 0 && settings.BikeIdMin > settings.BikeIdMax)
        {
            throw new InvalidOperationException("LeaseGeneration.BikeIdMin must be <= LeaseGeneration.BikeIdMax.");
        }

        if (settings.RenterIds.Count == 0 && settings.RenterIdMin > settings.RenterIdMax)
        {
            throw new InvalidOperationException("LeaseGeneration.RenterIdMin must be <= LeaseGeneration.RenterIdMax.");
        }

        if (settings.BikeIds.Any(id => id <= 0))
        {
            throw new InvalidOperationException("LeaseGeneration.BikeIds must contain only positive IDs.");
        }

        if (settings.RenterIds.Any(id => id <= 0))
        {
            throw new InvalidOperationException("LeaseGeneration.RenterIds must contain only positive IDs.");
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

    private static int PickId(IReadOnlyList<int> ids, int min, int max)
    {
        if (ids.Count > 0)
        {
            return ids[Random.Shared.Next(ids.Count)];
        }

        return Random.Shared.Next(min, max + 1);
    }
}
