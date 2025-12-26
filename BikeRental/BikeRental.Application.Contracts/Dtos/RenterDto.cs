namespace BikeRental.Application.Contracts.Dtos;

/// <summary>
///     A class describing a renter
/// </summary>
public class RenterDto
{
    /// <summary>
    ///     Renter's id
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///     Renter's full name
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    ///     Renter's phone number
    /// </summary>
    public required string PhoneNumber { get; set; }
}