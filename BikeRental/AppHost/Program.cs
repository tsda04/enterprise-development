using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ContainerResource> nats = builder.AddContainer("nats", "nats:2.10")
    .WithArgs("-js")
    .WithEndpoint(4222, 4222);

IResourceBuilder<ParameterResource> bikeRentalDbPassword = builder.AddParameter(
    "bike-rental-db-password",
    "1234512345Aa$",
    secret: true);
IResourceBuilder<MySqlServerResource> bikeRentalSql = builder.AddMySql("bike-rental-db",
        bikeRentalDbPassword)
    .WithAdminer()
    .WithDataVolume("bike-rental-volume");

IResourceBuilder<MySqlDatabaseResource> bikeRentalDb =
    bikeRentalSql.AddDatabase("bike-rental");

builder.AddProject<BikeRental_Generator_Nats_Host>("bike-rental-nats-generator")
    .WaitFor(nats)
    .WithEnvironment("Nats__Url", "nats://localhost:4222")
    .WithEnvironment("Nats__StreamName", "bike-rental-stream")
    .WithEnvironment("Nats__SubjectName", "bike-rental.leases");

builder.AddProject<BikeRental_Api>("bike-rental-api")
    .WaitFor(bikeRentalDb)
    .WaitFor(nats)
    .WithReference(bikeRentalDb)
    .WithEnvironment("Nats__Url", "nats://localhost:4222")
    .WithEnvironment("Nats__StreamName", "bike-rental-stream")
    .WithEnvironment("Nats__SubjectName", "bike-rental.leases");

builder.Build().Run();