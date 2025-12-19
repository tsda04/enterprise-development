namespace BikeRental.Application.Contracts.Dtos;

/// <summary>
/// A class describing a bike for rent
/// </summary>
public class BikeDto
{
    /// <summary>
    /// Bike's unique id
    /// </summary>
    public required int Id { get; set; }

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
    public required int ModelId { get; set; }
    

}