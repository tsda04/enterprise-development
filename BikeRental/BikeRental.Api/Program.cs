using BikeRental.Api;
using BikeRental.Api.Extensions;
using Microsoft.OpenApi.Models;

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

var app = builder.Build();

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

app.UseExceptionHandler();

app.MapControllers(); 

await app.RunAsync().ConfigureAwait(false);