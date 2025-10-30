namespace BikeRental.Domain.Models;

/// <summary>
/// A class describing a bike for rent
/// </summary>
public class Bike
{
    /// <summary>
    /// Bike's unique id
    /// </summary>
    public required Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Bike's serial number
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Bike's color
    /// </summary>
    public required string Color { get; set; }

    /// <summary>
    /// Bike's model
    /// </summary>
    public required BikeModel Model { get; set; }
}
