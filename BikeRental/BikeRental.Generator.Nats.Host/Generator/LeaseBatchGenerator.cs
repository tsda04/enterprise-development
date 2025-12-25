using Bogus;
using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Generator.Nats.Host.Generator;

public sealed class LeaseBatchGenerator
{
    public IList<LeaseCreateUpdateDto> GenerateBatch(LeaseGenerationOptions settings)
    {
        Validate(settings);

        var faker = CreateFaker(settings);
        return faker.Generate(settings.BatchSize);
    }

    private static Faker<LeaseCreateUpdateDto> CreateFaker(
        LeaseGenerationOptions settings)
    {
        return new Faker<LeaseCreateUpdateDto>()
            .RuleFor(x => x.RenterId, f => 
                f.Random.Int(settings.RenterIdMin, settings.RenterIdMax))
            .RuleFor(x => x.BikeId, f => 
                f.Random.Int(settings.BikeIdMin, settings.BikeIdMax))
            .RuleFor(x => x.RentalDuration, f =>
                f.Random.Int(
                    settings.RentalDurationMinHours,
                    settings.RentalDurationMaxHours))
            .RuleFor(x => x.RentalStartTime, f =>
                GeneratePastStartTime(
                    settings.RentalStartDaysBackMax,
                    f));
    }

    private static DateTime GeneratePastStartTime(
        int maxDaysBack,
        Faker f)
    {
        var daysBack = f.Random.Int(0, maxDaysBack);
        var hoursBack = f.Random.Int(0, 23);

        return DateTime.UtcNow
            .AddDays(-daysBack)
            .AddHours(-hoursBack);
    }

    private static void Validate(LeaseGenerationOptions settings)
    {
        if (settings.BatchSize <= 0)
            throw new InvalidOperationException("BatchSize must be > 0.");

        if (settings.BikeIdMin > settings.BikeIdMax)
            throw new InvalidOperationException(
                "BikeIdMin must be <= BikeIdMax.");

        if (settings.RenterIdMin > settings.RenterIdMax)
            throw new InvalidOperationException(
                "RenterIdMin must be <= RenterIdMax.");

        if (settings.RentalDurationMinHours >
            settings.RentalDurationMaxHours)
            throw new InvalidOperationException(
                "RentalDurationMinHours must be <= RentalDurationMaxHours.");

        if (settings.RentalStartDaysBackMax < 0)
            throw new InvalidOperationException(
                "RentalStartDaysBackMax must be >= 0.");
    }
}