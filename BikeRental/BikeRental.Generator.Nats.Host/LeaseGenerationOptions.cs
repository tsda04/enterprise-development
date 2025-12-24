namespace BikeRental.Generator.Nats.Host;

// нет проверки на существование велика и арендатора с таким айди
public sealed class LeaseGenerationOptions
{
    public int BatchSize { get; init; }
    public int IntervalSeconds { get; init; }
    public int BikeIdMin { get; init; }
    public int BikeIdMax { get; init; }
    public int RenterIdMin { get; init; }
    public int RenterIdMax { get; init; }
    public int RentalDurationMinHours { get; init; }
    public int RentalDurationMaxHours { get; init; }
    public int RentalStartDaysBackMax { get; init; } 
}
