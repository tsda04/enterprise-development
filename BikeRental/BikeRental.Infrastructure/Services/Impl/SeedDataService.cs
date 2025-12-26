using BikeRental.Domain.Enum;
using BikeRental.Domain.Models;
using BikeRental.Infrastructure.Database;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Services.Impl;

/// <summary>
///     Сервис инициализации данных
/// </summary>
/// <param name="dbContext"></param>
public class SeedDataService(ApplicationDbContext dbContext) : ISeedDataService
{
    /// <summary>
    ///     Выполнить инициализацию данных
    /// </summary>
    public async Task SeedDataAsync()
    {
        // Подготовить генератор фейковых данных
        var faker = new Faker("ru");

        // Создать модели велосипедов, если они отсутствуют
        if (!await dbContext.BikeModels.AnyAsync())
        {
            var bikeModels = new List<BikeModel>();
            for (var i = 0; i < 10; i++)
            {
                var bikeModel = new BikeModel
                {
                    Type = faker.PickRandom<BikeType>(),
                    WheelSize = faker.Random.Int(20, 29),
                    MaxCyclistWeight = faker.Random.Int(60, 120),
                    Weight = Math.Round(faker.Random.Double(7.0, 15.0), 2),
                    BrakeType = faker.PickRandom("Road", "Sport", "Mountain", "Hybrid"),
                    YearOfManufacture = faker.Date.Past(10).Year.ToString(),
                    RentPrice = Math.Round(faker.Random.Decimal(5.0m, 20.0m), 2)
                };
                bikeModels.Add(bikeModel);
            }

            dbContext.BikeModels.AddRange(bikeModels);
            await dbContext.SaveChangesAsync();
        }

        // Создать арендаторов, если они отсутствуют
        if (!await dbContext.Renters.AnyAsync())
        {
            var renters = new List<Renter>();
            for (var i = 0; i < 20; i++)
            {
                var renter = new Renter
                {
                    FullName = faker.Name.FullName(),
                    PhoneNumber = faker.Phone.PhoneNumber()
                };
                renters.Add(renter);
            }

            dbContext.Renters.AddRange(renters);
            await dbContext.SaveChangesAsync();
        }

        // Создать велосипеды, если они отсутствуют
        if (!await dbContext.Bikes.AnyAsync())
        {
            List<BikeModel> bikeModels = await dbContext.BikeModels.ToListAsync();
            var bikes = new List<Bike>();
            for (var i = 0; i < 30; i++)
            {
                var bike = new Bike
                {
                    ModelId = faker.PickRandom(bikeModels).Id,
                    SerialNumber = faker.Random.AlphaNumeric(10).ToUpper(),
                    Color = faker.Commerce.Color(),
                    Model = faker.PickRandom(bikeModels)
                };
                bikes.Add(bike);
            }

            dbContext.Bikes.AddRange(bikes);
            await dbContext.SaveChangesAsync();
        }

        // Создать договора аренды, если они отсутствуют для
        // некоторых велосипедов
        if (!await dbContext.Leases.AnyAsync())
        {
            List<Renter> renters = await dbContext.Renters.ToListAsync();
            List<Bike> bikes = await dbContext.Bikes.ToListAsync();
            var leases = new List<Lease>();

            for (var i = 0; i < 15; i++)
            {
                var lease = new Lease
                {
                    Renter = faker.PickRandom(renters),
                    Bike = faker.PickRandom(bikes),
                    RentalStartTime = faker.Date.Recent(10),
                    RentalDuration = faker.Random.Int(1, 72)
                };
                leases.Add(lease);
            }

            dbContext.Leases.AddRange(leases);
            await dbContext.SaveChangesAsync();
        }
    }
}