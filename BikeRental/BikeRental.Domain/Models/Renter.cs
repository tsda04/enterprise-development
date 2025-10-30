namespace BikeRental.Domain.Models;

/// <summary>
/// A class describing a bike for rent
/// </summary>
public class Renter
{
    /// <summary>
    /// Renter's id
    /// </summary>
    public required Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Renter's full name
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Renter's phone number
    /// </summary>
    public required string Number { get; set; }
}