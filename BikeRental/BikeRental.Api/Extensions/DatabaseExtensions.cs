using BikeRental.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Api.Extensions;

/// <summary>
/// Предоставляет методы расширения для работы с базой данных
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Применить все миграции к базе данных
    /// </summary>
    /// <param name="app"></param>
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        // мы не в HTTP запросе тк это запуск приложения
        // поэтому создаем Scope(один из уровней DI контейнера) вручную, как бы новую область видимости для DI
        // Scope гарантирует, что все зависимости будут правильно созданы и уничтожены
        using var scope = app.Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // scope.ServiceProvider - DI контейнер в рамках созданного Scope
        // GetRequiredService<T>() - получить сервис типа T
        // Требует, чтобы сервис был зарегистрирован, иначе исключение
        // DbContext реализует IAsyncDisposable (асинхронное освобождение ресурсов)

        try
        {
            await dbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database migrations.");
            throw;
        }
    }
}
