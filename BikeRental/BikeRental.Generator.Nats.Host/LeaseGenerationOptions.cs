namespace BikeRental.Generator.Nats.Host;

// нет проверки на существование велика и арендатора с таким айди
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
    public IReadOnlyList<int> BikeIds { get; init; } = Array.Empty<int>();
    public IReadOnlyList<int> RenterIds { get; init; } = Array.Empty<int>();
    public int LogBatchSampleCount { get; init; } = 0;
}
