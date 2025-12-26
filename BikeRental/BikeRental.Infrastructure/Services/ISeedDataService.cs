namespace BikeRental.Infrastructure.Services;

/// <summary>
///     Интерфейс описывает сервис инициализации данных
/// </summary>
public interface ISeedDataService
{
    /// <summary>
    ///     Выполнить инициализацию данных
    /// </summary>
    /// <returns></returns>
    public Task SeedDataAsync();
}