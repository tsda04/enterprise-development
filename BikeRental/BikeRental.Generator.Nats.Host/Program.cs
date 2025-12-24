using BikeRental.Generator.Nats.Host;
using Microsoft.Extensions.Options;
using NATS.Client.Core;

var builder = Host.CreateApplicationBuilder(args);


var natsSettingsSection = builder.Configuration.GetSection("NatsSettings");

if (natsSettingsSection.Exists())
{
    Console.WriteLine($"Nats.Url: {natsSettingsSection["Url"]}");
    Console.WriteLine($"Nats.StreamName: {natsSettingsSection["StreamName"]}");
}

builder.Services.Configure<NatsSettings>(builder.Configuration.GetSection("NatsSettings"));
builder.Services.Configure<LeaseGenerationOptions>(builder.Configuration.GetSection("LeaseGeneration"));

builder.Services.AddSingleton<INatsConnection>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<NatsSettings>>().Value;
    var options = new NatsOpts { Url = settings.Url };
    return new NatsConnection(options);
});

builder.Services.AddSingleton<LeaseBatchGenerator>();
builder.Services.AddSingleton<BikeRentalNatsProducer>();
builder.Services.AddHostedService<LeaseBatchWorker>();

var host = builder.Build();
await host.RunAsync();
//host.Run();