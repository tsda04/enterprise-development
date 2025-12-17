using BikeRental.Domain.Models;

namespace BikeRental.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория описывает контракт для работы с арендаторами
/// </summary>
public interface IRenterRepository : IRepository<Renter>
{
}