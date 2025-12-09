namespace BikeRental.Application.Contracts.Dtos;

public class BikeCreateUpdateDto
{
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