using BikeRental.Api.Middleware;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Services;
using BikeRental.Domain.Interfaces;
using BikeRental.Infrastructure.Database;
using BikeRental.Infrastructure.Repositories;
using BikeRental.Infrastructure.Services;
using BikeRental.Infrastructure.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BikeRental.Api;

/// <summary>
/// Настройка зависимостей приложения
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Зарегистрировать и настроить сервисы контроллеров
    /// </summary>
    public static void AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = false; // 406
            })
            .AddNewtonsoftJson(); // заменить стандартный JSON на Newtonsoft.json
        //.AddXmlSerializerFormatters(); // отвечать в XML формате
    }

    /// <summary>
    /// Зарегистрировать и настроить сервисы обработки ошибок
    /// </summary>
    public static void AddErrorHandling(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        builder.Services.AddProblemDetails();
    }

    /// <summary>
    /// Зарегистрировать и настроить сервисы OpenTelemetry
    /// </summary>
    public static void AddObservability(this WebApplicationBuilder builder)
    {
        // Зарегистрировать сервис OpenTelemetry
        builder.Services.AddOpenTelemetry()
            // Добавить ресурс по наименованию приложения
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
            // Настроить распределенную трассировку
            .WithTracing(tracing => tracing
                // Добавить инструментарии для HttpClient и ASP.NET Core
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation())
            // Добавить метрики
            .WithMetrics(metrics => metrics
                // Добавить инструментарии для .NET Runtime, HttpClient и ASP.NET Core
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation())
            // Настроить глобальный экспортер метрик
            .UseOtlpExporter();

        // Настроить ведение журнала OpenTelemetry
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;           // Включить области
            options.IncludeFormattedMessage = true; // Включить форматированные сообщения
        });
        
    }

    /// <summary>
    /// Зарегистрировать и настроить сервисы взаимодействия с базой данных
    /// </summary>
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseMySQL(
                    builder.Configuration.GetConnectionString("bike-rental") ?? throw new InvalidOperationException(),
                    npgsqlOptions => npgsqlOptions
                        // Настроить таблицу истории миграций
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName))
                // Использовать соглашение именования snake_case
                .UseSnakeCaseNamingConvention());
    }

    /// <summary>
    /// Зарегистрировать репозитории
    /// </summary>
    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBikeModelRepository, BikeModelRepository>();
        builder.Services.AddScoped<IBikeRepository, BikeRepository>();
        builder.Services.AddScoped<IRenterRepository, RenterRepository>();
        builder.Services.AddScoped<ILeaseRepository, LeaseRepository>();
    }

    /// <summary>
    /// Регистрация сервисов общего назначения
    /// </summary>
    public static void AddServices(this WebApplicationBuilder builder)
    {
        // Зарегистрировать сервис инициализации данных
        builder.Services.AddScoped<ISeedDataService, SeedDataService>();
        
        // Зарегистрировать сервисы прикладного уровня
        builder.Services.AddScoped<IBikeModelService, BikeModelService>();
        builder.Services.AddScoped<IBikeService, BikeService>();
        builder.Services.AddScoped<IRenterService, RenterService>();
        builder.Services.AddScoped<ILeaseService, LeaseService>();
    }
}
