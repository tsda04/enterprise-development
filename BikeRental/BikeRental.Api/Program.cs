using BikeRental.Api;
using BikeRental.Api.Messaging;
using BikeRental.Api.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using NATS.Client.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddControllers();
builder.AddErrorHandling();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BikeRental API",
        Version = "v1",
        Description = "API для управления сервисом проката велосипедов"
    });
    
    var basePath = AppContext.BaseDirectory;
    var xmlPathApi = Path.Combine(basePath, $"BikeRental.Api.xml");
    options.IncludeXmlComments(xmlPathApi);
});


builder.AddObservability();
builder.AddDatabase();
builder.AddRepositories();
builder.AddServices();

var natsSettingsSection = builder.Configuration.GetSection("NatsConsumerSettings");

if (natsSettingsSection.Exists())
{
    Console.WriteLine($"Nats.Url: {natsSettingsSection["Url"]}");
    Console.WriteLine($"Nats.StreamName: {natsSettingsSection["StreamName"]}");
}

builder.Services.Configure<NatsConsumerSettings>(builder.Configuration.GetSection("NatsConsumerSettings"));
builder.Services.AddSingleton<INatsConnection>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<NatsConsumerSettings>>().Value;
    var connectRetryDelayMs = Math.Max(0, settings.ConnectRetryDelayMs);
    var reconnectWaitMin = TimeSpan.FromMilliseconds(connectRetryDelayMs);
    var reconnectWaitMax = TimeSpan.FromMilliseconds(
        Math.Max(connectRetryDelayMs, connectRetryDelayMs * Math.Max(settings.RetryBackoffFactor, 1)));

    var options = new NatsOpts
    {
        Url = settings.Url,
        RetryOnInitialConnect = settings.ConnectRetryAttempts > 1,
        MaxReconnectRetry = Math.Max(0, settings.ConnectRetryAttempts),
        ReconnectWaitMin = reconnectWaitMin,
        ReconnectWaitMax = reconnectWaitMax,
        ReconnectJitter = TimeSpan.FromMilliseconds(connectRetryDelayMs * 0.2)
    };
    return new NatsConnection(options);
});
builder.Services.AddHostedService<NatsLeaseConsumer>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // https://localhost:<port>/swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BikeRental API v1");
        c.RoutePrefix = "swagger";
        c.ShowCommonExtensions();
    });
    
    await app.ApplyMigrationsAsync();
    
    await app.SeedData();
}

app.MapControllers(); 

await app.RunAsync();
