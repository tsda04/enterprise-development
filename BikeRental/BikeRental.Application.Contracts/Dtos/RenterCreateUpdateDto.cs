namespace BikeRental.Application.Contracts.Dtos;

public class RenterCreateUpdateDto
{
    /// <summary>
    /// Renter's full name
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Renter's phone number
    /// </summary>
    public required string PhoneNumber { get; set; }
}