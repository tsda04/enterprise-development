using BikeRental.Infrastructure.Services;

namespace BikeRental.Api.Extensions;

/// <summary>
///     Предоставляет методы расширения для выполнения
///     первичной инициализации данных в бд
/// </summary>
public static class SeedDataExtensions
{
    /// <summary>
    ///     Проинициализировать данные в бд
    /// </summary>
    /// <param name="app"></param>
    public static async Task SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ISeedDataService seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
        await seedDataService.SeedDataAsync();
    }
}