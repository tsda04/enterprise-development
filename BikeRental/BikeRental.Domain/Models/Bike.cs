using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRental.Domain.Models;

/// <summary>
/// A class describing a bike for rent
/// </summary>
public class Bike
{
    /// <summary>
    /// Bike's unique id
    /// </summary>
    public int Id { get; set; }
    
    [ForeignKey(nameof(Model))]
    public required int ModelId { get; set; }

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
    public required BikeModel Model { get; init; } = null!;
}