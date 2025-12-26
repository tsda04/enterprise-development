namespace BikeRental.Generator.Nats.Host;

/// <summary>
///     Класс для типизированной конфигурации
/// </summary>
public sealed class NatsSettings
{
    public string Url { get; init; } = "nats://localhost:4222";
    public string StreamName { get; init; } = string.Empty;
    public string SubjectName { get; init; } = string.Empty;
    public int ConnectRetryAttempts { get; init; } = 5;
    public int ConnectRetryDelayMs { get; init; } = 2000;
    public int PublishRetryAttempts { get; init; } = 3;
    public int PublishRetryDelayMs { get; init; } = 1000;
    public double RetryBackoffFactor { get; init; } = 2;
}