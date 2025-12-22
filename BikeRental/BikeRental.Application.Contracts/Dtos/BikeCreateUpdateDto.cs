using System.ComponentModel.DataAnnotations;

namespace BikeRental.Application.Contracts.Dtos;

public class BikeCreateUpdateDto
{
    /// <summary>
    /// Bike's serial number
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина SerialNumber должна быть 3-50 символов.")]
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Bike's color
    /// </summary>
    [Required]
    [StringLength(20, ErrorMessage = "Макс. длина Color 20 символов.")]
    public required string Color { get; set; }

    /// <summary>
    /// Bike's model
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ModelId должно быть положительное число.")]
    public required int ModelId { get; set; }
}