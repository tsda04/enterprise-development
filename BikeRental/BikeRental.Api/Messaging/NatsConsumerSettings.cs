namespace BikeRental.Api.Messaging;

internal sealed class NatsConsumerSettings
{
    public string Url { get; init; } = "nats://localhost:4222";
    public string StreamName { get; init; } = string.Empty;
    public string SubjectName { get; init; } = string.Empty;
    public string DurableName { get; init; } = "bike-rental-lease-consumer";
    public int AckWaitSeconds { get; init; } = 30;
    public long MaxDeliver { get; init; } = 5;
    public int ConnectRetryAttempts { get; init; } = 5;
    public int ConnectRetryDelayMs { get; init; } = 2000;
    public double RetryBackoffFactor { get; init; } = 2;
    public int ConsumeMaxMsgs { get; init; } = 100;
    public int ConsumeExpiresSeconds { get; init; } = 30;
    public int ConsumeRetryDelayMs { get; init; } = 2000;
}