using System.ComponentModel.DataAnnotations;

namespace BikeRental.Application.Contracts.Dtos;

public class LeaseCreateUpdateDto
{
    /// <summary>
    /// Person who rents a bike
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ID человека должно быть положительное число.")]
    public required int RenterId { get; set; }

    /// <summary>
    /// Bike for rent
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ID велика должно быть положительное число.")]
    public required int BikeId { get; set; }

    /// <summary>
    /// Rental start time
    /// </summary>
    [Required]
    public required DateTime RentalStartTime { get; set; }

    /// <summary>
    /// Rental duration in hours
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Время должно быть от часа.")]
    public required int RentalDuration { get; set; }
}