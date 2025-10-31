namespace BikeRental.Domain.Models;

/// <summary>
/// A class describing a lease agreement
/// </summary>
public class Lease
{
    /// <summary>
    /// Person who rents a bike
    /// </summary>
    public required Renter Renter { get; set; }

    /// <summary>
    /// Bike for rent
    /// </summary>
    public required Bike Bike { get; set; }
    
    /// <summary>
    /// Lease ID
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Rental start time
    /// </summary>
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration in hours
    /// </summary>
    public required int RentalDuration { get; set; }
}