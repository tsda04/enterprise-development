using BikeRental.Domain.Models;

namespace BikeRental.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория описывает контракт для работы с велосипедами
/// </summary>
public interface IBikeRepository : IRepository<Bike>
{
}