using System.ComponentModel.DataAnnotations;

namespace BikeRental.Application.Contracts.Dtos;

public class RenterCreateUpdateDto
{
    /// <summary>
    /// Renter's full name
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина 3-100 символов.")]
    public required string FullName { get; set; }

    /// <summary>
    /// Renter's phone number
    /// </summary>
    [Required]
    [Phone(ErrorMessage = "Неверный формат телефона.")]
    public required string PhoneNumber { get; set; }
}