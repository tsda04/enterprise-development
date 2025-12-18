var builder = DistributedApplication.CreateBuilder(args);

var bikeRentalDbPassword = builder.AddParameter(
    name: "bike-rental-db-password",
    value: "1234512345Aa$",
    secret: true);
var bikeRentalSql = builder.AddMySql("bike-rental-db",
        password: bikeRentalDbPassword)
    .WithAdminer()
    .WithDataVolume("bike-rental-volume");

var bikeRentalDb =
    bikeRentalSql.AddDatabase("bike-rental");

builder.AddProject<Projects.BikeRental_Api>("bike-rental-api")
    .WaitFor(bikeRentalDb)
    .WithReference(bikeRentalDb);

builder.Build().Run();