using System.ComponentModel.DataAnnotations;
using BikeRental.Domain.Enum;

namespace BikeRental.Application.Contracts.Dtos;

public class BikeModelCreateUpdateDto
{
    /// <summary>
    ///     The type of bicycle: road, sport, mountain, hybrid
    /// </summary>
    [Required]
    public required BikeType Type { get; set; }

    /// <summary>
    ///     The size of the bicycle's wheels
    /// </summary>
    [Required]
    [Range(12, 36, ErrorMessage = "Размер колес должен быть 12-36.")]
    public required int WheelSize { get; set; }

    /// <summary>
    ///     Maximum permissible cyclist weight
    /// </summary>
    [Required]
    [Range(30, 200, ErrorMessage = "Вес человека должен быть 30-200 кг.")]
    public required int MaxCyclistWeight { get; set; }

    /// <summary>
    ///     Weight of the bike model
    /// </summary>
    [Required]
    [Range(3.0, 50.0, ErrorMessage = "Вес байка 3-50 кг.")]
    public required double Weight { get; set; }

    /// <summary>
    ///     The type of braking system used in this model of bike
    /// </summary>
    [Required]
    [StringLength(30, ErrorMessage = "Макс. длина 30 символов.")]
    public required string BrakeType { get; set; }

    /// <summary>
    ///     Year of manufacture of the bicycle model
    /// </summary>
    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Год должен быть 4 цифры.")]
    public required string YearOfManufacture { get; set; }

    /// <summary>
    ///     Cost per hour rental
    /// </summary>
    [Required]
    [Range(0.01, 1000, ErrorMessage = "Цена должна быть > 0.")]
    public required decimal RentPrice { get; set; }
}