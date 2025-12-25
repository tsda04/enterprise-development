namespace BikeRental.Generator.Nats.Host.Generator;

public sealed class LeaseGenerationOptions
{
    public int BatchSize { get; init; } = 10;
    public int IntervalSeconds { get; init; } = 30;
    public int BikeIdMin { get; init; } = 1;
    public int BikeIdMax { get; init; } = 30;
    public int RenterIdMin { get; init; } = 1;
    public int RenterIdMax { get; init; } = 20;
    public int RentalDurationMinHours { get; init; } = 1;
    public int RentalDurationMaxHours { get; init; } = 72;
    public int RentalStartDaysBackMax { get; init; } = 10;
}
