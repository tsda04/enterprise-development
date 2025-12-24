var builder = DistributedApplication.CreateBuilder(args);

var nats = builder.AddContainer("nats", "nats:2.10")
    .WithArgs("-js")
    .WithEndpoint(4222, 4222);

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

builder.AddProject<Projects.BikeRental_Generator_Nats_Host>("bike-rental-nats-generator")
    .WaitFor(nats)
    .WithEnvironment("Nats__Url", "nats://localhost:4222")
    .WithEnvironment("Nats__StreamName", "bike-rental-stream")
    .WithEnvironment("Nats__SubjectName", "bike-rental.leases");

builder.AddProject<Projects.BikeRental_Api>("bike-rental-api")
    .WaitFor(bikeRentalDb)
    .WaitFor(nats)
    .WithReference(bikeRentalDb)
    .WithEnvironment("Nats__Url", "nats://localhost:4222")
    .WithEnvironment("Nats__StreamName", "bike-rental-stream")
    .WithEnvironment("Nats__SubjectName", "bike-rental.leases");

builder.Build().Run();
