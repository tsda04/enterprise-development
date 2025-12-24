namespace BikeRental.Generator.Nats.Host;
// для типизированной конфигурации
public sealed class NatsSettings
{
    public string Url { get; init; } = "nats://localhost:4222";
    public string StreamName { get; init; } = string.Empty;
    public string SubjectName { get; init; } = string.Empty;
}
