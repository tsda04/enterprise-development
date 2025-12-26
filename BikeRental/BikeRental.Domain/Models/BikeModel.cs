using BikeRental.Domain.Enum;

namespace BikeRental.Domain.Models;

/// <summary>
///     A class describing the models of bikes that can be rented
/// </summary>
public class BikeModel
{
    /// <summary>
    ///     The unique id for bike model
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The type of bicycle: road, sport, mountain, hybrid
    /// </summary>
    public required BikeType Type { get; set; }

    /// <summary>
    ///     The size of the bicycle's wheels
    /// </summary>
    public required int WheelSize { get; set; }

    /// <summary>
    ///     Maximum permissible cyclist weight
    /// </summary>
    public required int MaxCyclistWeight { get; set; }

    /// <summary>
    ///     Weight of the bike model
    /// </summary>
    public required double Weight { get; set; }

    /// <summary>
    ///     The type of braking system used in this model of bike
    /// </summary>
    public required string BrakeType { get; set; }

    /// <summary>
    ///     Year of manufacture of the bicycle model
    /// </summary>
    public required string YearOfManufacture { get; set; }

    /// <summary>
    ///     Cost per hour rental
    /// </summary>
    public required decimal RentPrice { get; set; }
}