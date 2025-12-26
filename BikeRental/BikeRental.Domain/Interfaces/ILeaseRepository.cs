using BikeRental.Domain.Models;

namespace BikeRental.Domain.Interfaces;

/// <summary>
///     Интерфейс репозитория описывает контракт для работы с договорами на аренду велосипедов
/// </summary>
public interface ILeaseRepository : IRepository<Lease>;