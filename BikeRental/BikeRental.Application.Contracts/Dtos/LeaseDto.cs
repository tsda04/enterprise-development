namespace BikeRental.Application.Contracts.Dtos;

/// <summary>
/// A class describing a lease agreement
/// </summary>
public class LeaseDto
{
    /// <summary>
    /// Lease ID
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Person who rents a bike
    /// </summary>
    public required int RenterId { get; set; }

    /// <summary>
    /// Bike for rent
    /// </summary>
    public required int BikeId { get; set; }

    /// <summary>
    /// Rental start time
    /// </summary>
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration in hours
    /// </summary>
    public required int RentalDuration { get; set; }
    
}