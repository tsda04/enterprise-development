using BikeRental.Api;
using BikeRental.Api.Extensions;
using Microsoft.OpenApi.Models;

// Создать объект WebApplicationBuilder (построитель веб-приложения)
// с использованием переданных аргументов командной строки
var builder = WebApplication.CreateBuilder(args);
// Зарегистрировать и настроить сервисы контроллеров
builder.AddControllers();
// Зарегистрировать и настроить сервисы обработки ошибок
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
    
    // описание XML документации
    var basePath = AppContext.BaseDirectory;
    var xmlPathApi = Path.Combine(basePath, $"BikeRental.Api.xml");
    options.IncludeXmlComments(xmlPathApi);
});


// Зарегистрировать и настроить сервисы OpenTelemetry 
builder.AddObservability();
// Зарегистрировать и настроить сервисы взаимодействия с базой данных
builder.AddDatabase();
// Зарегистрировать и настроить сервисы репозиториев
builder.AddRepositories();
// Зарегистрировать и настроить сервисы общего назначения
builder.AddServices();

// Создать конвейер обработки запросов
var app = builder.Build();

// Если приложение работает в режиме разработки, то
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

    
    // Применить миграции базы данных (из DatabaceExtensions)
    await app.ApplyMigrationsAsync();
    
    // Инициализировать данные в бд
    await app.SeedData();
}

// Использовать обработчики исключений (GlobalExceptionHandler, ValidationExceptionHandler)
app.UseExceptionHandler();

// Зарегистрировать конечные точки контроллеров
app.MapControllers(); 
// Запустить приложение
await app.RunAsync().ConfigureAwait(false); 