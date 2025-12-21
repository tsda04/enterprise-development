using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRental.Domain.Models;

/// <summary>
/// A class describing a lease agreement
/// </summary>
public class Lease
{
    /// <summary>
    /// Lease ID
    /// </summary>
    public int Id { get; set; }
    
    [ForeignKey(nameof(Bike))]
    public int BikeId { get; set; }
    
    [ForeignKey(nameof(Renter))]
    public int RenterId { get; set; }

    /// <summary>
    /// Rental start time
    /// </summary>
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration in hours
    /// </summary>
    public required int RentalDuration { get; set; }
    
    /// <summary>
    /// Person who rents a bike
    /// </summary>
    public required Renter Renter { get; set; } 

    /// <summary>
    /// Bike for rent
    /// </summary>
    public required Bike Bike { get; set; }
    // сделала required тогда их айди автоматически должны установиться EF core
}