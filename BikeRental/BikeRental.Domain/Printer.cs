using BikeRental.Domain.Models;
using BikeRental.Domain.Enum;

namespace BikeRental.Domain;
/// <summary>
/// A class just for print initialized data
/// </summary>
public class Printer
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Bike initialization");
        
        BikeModel roadBikeModel = new BikeModel
        {
            Id = Guid.NewGuid(),
            Type = BikeType.Road,
            WheelSize = 700,
            MaxСyclistWeight = 100,
            Weight = 10,
            BrakeType = "Disc",
            YearOfManufacture = "2023",
            RentPrice = 15
        };

        Bike myBike = new Bike
        {
            Id = Guid.NewGuid(),
            SerialNumber = "SN1234567890",
            Color = "Red",
            Model = roadBikeModel
        };
        
        Renter renter1 = new Renter
        {
            Id = Guid.NewGuid(),
            FullName = "Иван Иванов",
            PhoneNumber = "+7 (999) 123-45-67"
        };

        Lease lease1 = new Lease
        {
            Id = Guid.NewGuid(),
            Renter = renter1,
            Bike = myBike,
            RentalStartTime = DateTime.Now,
            RentalDuration = 3
        };

        Console.WriteLine("\nBike Information");
        Console.WriteLine($"Bike ID: {myBike.Id}");
        Console.WriteLine($"Serial Number: {myBike.SerialNumber}");
        Console.WriteLine($"Color: {myBike.Color}");
        
        Console.WriteLine("\nBike Model Information");
        Console.WriteLine($"  Model ID: {myBike.Model.Id}");
        Console.WriteLine($"  Type: {myBike.Model.Type}");
        Console.WriteLine($"  Wheel Size: {myBike.Model.WheelSize}");
        Console.WriteLine($"  Max Passenger Weight: {myBike.Model.MaxСyclistWeight}");
        Console.WriteLine($"  Weight: {myBike.Model.Weight}");
        Console.WriteLine($"  Brake Type: {myBike.Model.BrakeType}");
        Console.WriteLine($"  Year: {myBike.Model.YearOfManufacture}");
        Console.WriteLine($"  Rent Price (per hour): {myBike.Model.RentPrice}");
        
        Console.WriteLine($"Full Name: {renter1.FullName}");
        Console.WriteLine($"Phone Number: {renter1.PhoneNumber}");

        Console.WriteLine("\nLease Agreement Information");
        Console.WriteLine($"Lease ID: {lease1.Id}");
        Console.WriteLine($"Renter: {lease1.Renter.FullName} (ID: {lease1.Renter.Id})");
        Console.WriteLine($"Bike Serial Number: {lease1.Bike.SerialNumber} (Color: {lease1.Bike.Color})");
        Console.WriteLine($"Rental Start Time: {lease1.RentalStartTime}");
        Console.WriteLine($"Rental Duration (hours): {lease1.RentalDuration}");
        Console.WriteLine($"Estimated Rental Cost: {lease1.RentalDuration * lease1.Bike.Model.RentPrice} (currency units)");


        Console.WriteLine("\nInitialization complete.");
    }
}
