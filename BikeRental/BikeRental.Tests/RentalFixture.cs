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
    public List<Lease> Lease { get; }
    
    /// <summary>
    /// A class for creating the data for testing
    /// </summary>
    public RentalFixture()
    {
        Models = GetBikeModels();
        Renters = GetRenters();
        Bikes = GetBikes(Models);
        Lease = GetLeases(Bikes, Renters);
    }
    
    private List<BikeModel> GetBikeModels() =>
    [
        new() { Id = 1, Type = BikeType.Mountain, WheelSize = 26, MaxСyclistWeight = 95, Weight = 8.2, BrakeType = "Carbon", YearOfManufacture = "2024", RentPrice = 18 },
        new() { Id = 2, Type = BikeType.Road, WheelSize = 27, MaxСyclistWeight = 115, Weight = 12.8, BrakeType = "Hydraulic", YearOfManufacture = "2023", RentPrice = 25 },
        new() { Id = 3, Type = BikeType.Sport, WheelSize = 28, MaxСyclistWeight = 85, Weight = 7.9, BrakeType = "V-Brake", YearOfManufacture = "2024", RentPrice = 22 },
        new() { Id = 4, Type = BikeType.Road, WheelSize = 29, MaxСyclistWeight = 105, Weight = 14.7, BrakeType = "Mechanical", YearOfManufacture = "2023", RentPrice = 20 },
        new() { Id = 5, Type = BikeType.Hybrid, WheelSize = 26, MaxСyclistWeight = 90, Weight = 6.8, BrakeType = "Hydraulic", YearOfManufacture = "2024", RentPrice = 35 },
        new() { Id = 6, Type = BikeType.Sport, WheelSize = 28, MaxСyclistWeight = 125, Weight = 13.5, BrakeType = "Disc", YearOfManufacture = "2023", RentPrice = 28 },
        new() { Id = 7, Type = BikeType.Mountain, WheelSize = 27, MaxСyclistWeight = 110, Weight = 12.2, BrakeType = "V-Brake", YearOfManufacture = "2022", RentPrice = 16 },
        new() { Id = 8, Type = BikeType.Hybrid, WheelSize = 29, MaxСyclistWeight = 100, Weight = 7.5, BrakeType = "Carbon", YearOfManufacture = "2023", RentPrice = 32 },
        new() { Id = 9, Type = BikeType.Sport, WheelSize = 26, MaxСyclistWeight = 130, Weight = 15.8, BrakeType = "Hydraulic", YearOfManufacture = "2024", RentPrice = 24 },
        new() { Id = 10, Type = BikeType.Road, WheelSize = 28, MaxСyclistWeight = 80, Weight = 9.3, BrakeType = "Mechanical", YearOfManufacture = "2022", RentPrice = 19 },
    ];

    private List<Renter> GetRenters() =>
    [
        new() { Id = 1, FullName = "Алексеев Алексей", PhoneNumber = "+7 912 345 67 89" },
        new() { Id = 2, FullName = "Васильев Василий", PhoneNumber = "+7 923 456 78 90" },
        new() { Id = 3, FullName = "Григорьев Григорий", PhoneNumber = "+7 934 567 89 01" },
        new() { Id = 4, FullName = "Дмитриева Ольга", PhoneNumber = "+7 945 678 90 12" },
        new() { Id = 5, FullName = "Николаева Светлана", PhoneNumber = "+7 956 789 01 23" },
        new() { Id = 6, FullName = "Михайлов Сергей", PhoneNumber = "+7 967 890 12 34" },
        new() { Id = 7, FullName = "Романова Татьяна", PhoneNumber = "+7 978 901 23 45" },
        new() { Id = 8, FullName = "Павлов Дмитрий", PhoneNumber = "+7 989 012 34 56" },
        new() { Id = 9, FullName = "Фёдорова Екатерина", PhoneNumber = "+7 990 123 45 67" },
        new() { Id = 10, FullName = "Андреева Наталья", PhoneNumber = "+7 901 234 56 78" },
    ];

    private List<Bike> GetBikes(List<BikeModel> models) => 
    [
        new() { Id = 1, SerialNumber = "R001", Color = "Silver", Model = models[0] },
        new() { Id = 2, SerialNumber = "R002", Color = "Navy", Model = models[1] },
        new() { Id = 3, SerialNumber = "R003", Color = "Charcoal", Model = models[2] },
        new() { Id = 4, SerialNumber = "R004", Color = "Beige", Model = models[3] },
        new() { Id = 5, SerialNumber = "R005", Color = "Burgundy", Model = models[4] },
        new() { Id = 6, SerialNumber = "R006", Color = "Teal", Model = models[5] },
        new() { Id = 7, SerialNumber = "R007", Color = "Coral", Model = models[6] },
        new() { Id = 8, SerialNumber = "R008", Color = "Indigo", Model = models[7] },
        new() { Id = 9, SerialNumber = "R009", Color = "Bronze", Model = models[8] },
        new() { Id = 10, SerialNumber = "R010", Color = "Lavender", Model = models[9] },
    ];

    private List<Lease> GetLeases(List<Bike> bikes, List<Renter> renters) => 
    [
        new() { Id = 1, Bike = bikes[0], Renter = renters[0], RentalStartTime = DateTime.Now.AddHours(-12), RentalDuration = 3 },
        new() { Id = 2, Bike = bikes[1], Renter = renters[1], RentalStartTime = DateTime.Now.AddHours(-8), RentalDuration = 6 },
        new() { Id = 3, Bike = bikes[2], Renter = renters[2], RentalStartTime = DateTime.Now.AddHours(-15), RentalDuration = 4 },
        new() { Id = 4, Bike = bikes[3], Renter = renters[3], RentalStartTime = DateTime.Now.AddHours(-5), RentalDuration = 2 },
        new() { Id = 5, Bike = bikes[4], Renter = renters[4], RentalStartTime = DateTime.Now.AddHours(-20), RentalDuration = 8 },
        new() { Id = 6, Bike = bikes[5], Renter = renters[5], RentalStartTime = DateTime.Now.AddHours(-3), RentalDuration = 1 },
        new() { Id = 7, Bike = bikes[6], Renter = renters[6], RentalStartTime = DateTime.Now.AddHours(-18), RentalDuration = 5 },
        new() { Id = 8, Bike = bikes[7], Renter = renters[7], RentalStartTime = DateTime.Now.AddHours(-7), RentalDuration = 7 },
        new() { Id = 9, Bike = bikes[8], Renter = renters[8], RentalStartTime = DateTime.Now.AddHours(-10), RentalDuration = 4 },
        new() { Id = 10, Bike = bikes[9], Renter = renters[9], RentalStartTime = DateTime.Now.AddHours(-2), RentalDuration = 3 },
    ];
}