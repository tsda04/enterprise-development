using BikeRental.Domain.Models;

namespace BikeRental.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория описывает контракт для работы с моделями велосипедов
/// </summary>
public interface IBikeModelRepository : IRepository<BikeModel>;