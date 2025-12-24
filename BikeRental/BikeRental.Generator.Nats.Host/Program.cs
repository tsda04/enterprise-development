using BikeRental.Generator.Nats.Host;

// TODO тут подключение к NATS + генератор и воркер 

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
