namespace BikeRental.Domain.Models;

/// <summary>
///     A class describing a renter
/// </summary>
public class Renter
{
    /// <summary>
    ///     Renter's id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Renter's full name
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    ///     Renter's phone number
    /// </summary>
    public required string PhoneNumber { get; set; }
}