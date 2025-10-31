using BikeRental.Domain.Enum;
using BikeRental.Domain.Models;

namespace BikeRental.Tests;

public class RentalFixture
{
    /// <summary>
    /// A list of all bike models
    /// </summary>
    public List<BikeModel> Models { get; }

    /// <summary>
    /// /// A list of all bikes for rent
    /// </summary>
    public List<Bike> Bikes { get; }
    
    /// <summary>
    /// A list of all registered renters
    /// </summary>
    public List<Renter> Renters { get; }

    /// <summary>
    /// A list of all leases
    /// </summary>
    public readonly List<Lease> Lease;
    

    public RentalFixture()
    {
        Models = GetBikeModels();
        Renters = GetRenters();
        Bikes = GetBikes(Models);
        Lease = GetRentals(Bikes, Renters);
    }

    private List<BikeModel> GetBikeModels() =>
    [
        new() { Id = Guid.NewGuid(), Type = BikeType.Mountain, WheelSize = 26, MaxСyclistWeight = 95, Weight = 8.2, BrakeType = "Carbon", YearOfManufacture = "2024", RentPrice = 18 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Road, WheelSize = 27, MaxСyclistWeight = 115, Weight = 12.8, BrakeType = "Hydraulic", YearOfManufacture = "2023", RentPrice = 25 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Sport, WheelSize = 28, MaxСyclistWeight = 85, Weight = 7.9, BrakeType = "V-Brake", YearOfManufacture = "2024", RentPrice = 22 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Road, WheelSize = 29, MaxСyclistWeight = 105, Weight = 14.7, BrakeType = "Mechanical", YearOfManufacture = "2023", RentPrice = 20 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Hybrid, WheelSize = 26, MaxСyclistWeight = 90, Weight = 6.8, BrakeType = "Hydraulic", YearOfManufacture = "2024", RentPrice = 35 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Sport, WheelSize = 28, MaxСyclistWeight = 125, Weight = 13.5, BrakeType = "Disc", YearOfManufacture = "2023", RentPrice = 28 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Mountain, WheelSize = 27, MaxСyclistWeight = 110, Weight = 12.2, BrakeType = "V-Brake", YearOfManufacture = "2022", RentPrice = 16 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Hybrid, WheelSize = 29, MaxСyclistWeight = 100, Weight = 7.5, BrakeType = "Carbon", YearOfManufacture = "2023", RentPrice = 32 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Sport, WheelSize = 26, MaxСyclistWeight = 130, Weight = 15.8, BrakeType = "Hydraulic", YearOfManufacture = "2024", RentPrice = 24 },
        new() { Id = Guid.NewGuid(), Type = BikeType.Road, WheelSize = 28, MaxСyclistWeight = 80, Weight = 9.3, BrakeType = "Mechanical", YearOfManufacture = "2022", RentPrice = 19 },
    ];

    private List<Renter> GetRenters() =>
    [
        new() { Id = Guid.NewGuid(), FullName = "Алексеев Алексей", PhoneNumber = "+7 912 345 67 89" },
        new() { Id = Guid.NewGuid(), FullName = "Васильев Василий", PhoneNumber = "+7 923 456 78 90" },
        new() { Id = Guid.NewGuid(), FullName = "Григорьев Григорий", PhoneNumber = "+7 934 567 89 01" },
        new() { Id = Guid.NewGuid(), FullName = "Дмитриева Ольга", PhoneNumber = "+7 945 678 90 12" },
        new() { Id = Guid.NewGuid(), FullName = "Николаева Светлана", PhoneNumber = "+7 956 789 01 23" },
        new() { Id = Guid.NewGuid(), FullName = "Михайлов Сергей", PhoneNumber = "+7 967 890 12 34" },
        new() { Id = Guid.NewGuid(), FullName = "Романова Татьяна", PhoneNumber = "+7 978 901 23 45" },
        new() { Id = Guid.NewGuid(), FullName = "Павлов Дмитрий", PhoneNumber = "+7 989 012 34 56" },
        new() { Id = Guid.NewGuid(), FullName = "Фёдорова Екатерина", PhoneNumber = "+7 990 123 45 67" },
        new() { Id = Guid.NewGuid(), FullName = "Андреева Наталья", PhoneNumber = "+7 901 234 56 78" },
    ];

    private List<Bike> GetBikes(List<BikeModel> models) => 
    [
        new() { Id = Guid.NewGuid(), SerialNumber = "R001", Color = "Silver", Model = models[0] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R002", Color = "Navy", Model = models[1] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R003", Color = "Charcoal", Model = models[2] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R004", Color = "Beige", Model = models[3] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R005", Color = "Burgundy", Model = models[4] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R006", Color = "Teal", Model = models[5] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R007", Color = "Coral", Model = models[6] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R008", Color = "Indigo", Model = models[7] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R009", Color = "Bronze", Model = models[8] },
        new() { Id = Guid.NewGuid(), SerialNumber = "R010", Color = "Lavender", Model = models[9] },
    ];

    private List<Lease> GetRentals(List<Bike> bikes, List<Renter> renters) => 
    [
        new() { Id = Guid.NewGuid(), Bike = bikes[0], Renter = renters[0], RentalStartTime = DateTime.Now.AddHours(-12), RentalDuration = 3 },
        new() { Id = Guid.NewGuid(), Bike = bikes[1], Renter = renters[1], RentalStartTime = DateTime.Now.AddHours(-8), RentalDuration = 6 },
        new() { Id = Guid.NewGuid(), Bike = bikes[2], Renter = renters[2], RentalStartTime = DateTime.Now.AddHours(-15), RentalDuration = 4 },
        new() { Id = Guid.NewGuid(), Bike = bikes[3], Renter = renters[3], RentalStartTime = DateTime.Now.AddHours(-5), RentalDuration = 2 },
        new() { Id = Guid.NewGuid(), Bike = bikes[4], Renter = renters[4], RentalStartTime = DateTime.Now.AddHours(-20), RentalDuration = 8 },
        new() { Id = Guid.NewGuid(), Bike = bikes[5], Renter = renters[5], RentalStartTime = DateTime.Now.AddHours(-3), RentalDuration = 1 },
        new() { Id = Guid.NewGuid(), Bike = bikes[6], Renter = renters[6], RentalStartTime = DateTime.Now.AddHours(-18), RentalDuration = 5 },
        new() { Id = Guid.NewGuid(), Bike = bikes[7], Renter = renters[7], RentalStartTime = DateTime.Now.AddHours(-7), RentalDuration = 7 },
        new() { Id = Guid.NewGuid(), Bike = bikes[8], Renter = renters[8], RentalStartTime = DateTime.Now.AddHours(-10), RentalDuration = 4 },
        new() { Id = Guid.NewGuid(), Bike = bikes[9], Renter = renters[9], RentalStartTime = DateTime.Now.AddHours(-2), RentalDuration = 3 },
    ];
}