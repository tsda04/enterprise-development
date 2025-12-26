using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappings;
using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
///     Application-сервис для работы с договорами аренды велосипедов.
///     Инкапсулирует бизнес-логику и доступ к репозиторию.
///     На текущем этапе является тонкой обёрткой над ILeaseRepository.
/// </summary>
public sealed class LeaseService(
    ILeaseRepository leaseRepository,
    IBikeRepository bikeRepository,
    IRenterRepository renterRepository) : ILeaseService
{
    public async Task<IEnumerable<LeaseDto>> GetAll()
    {
        return (await leaseRepository.GetAll()).Select(l => l.ToDto());
    }

    public async Task<LeaseDto?> GetById(int id)
    {
        return (await leaseRepository.GetById(id))?.ToDto();
    }

    public async Task<LeaseDto> Create(LeaseCreateUpdateDto dto)
    {
        Bike bike = await bikeRepository.GetById(dto.BikeId)
                    ?? throw new ArgumentException($"Bike with id {dto.BikeId} not found.");

        Renter renter = await renterRepository.GetById(dto.RenterId)
                        ?? throw new ArgumentException($"Renter with id {dto.RenterId} not found.");

        var id = await leaseRepository.Add(dto.ToEntity(bike, renter));
        if (id > 0)
        {
            Lease? createdEntity = await leaseRepository.GetById(id);
            if (createdEntity != null)
            {
                return createdEntity.ToDto();
            }
        }

        throw new InvalidOperationException("Failed to create entity.");
    }

    public async Task<LeaseDto> Update(int id, LeaseCreateUpdateDto dto)
    {
        _ = await leaseRepository.GetById(id)
            ?? throw new KeyNotFoundException($"Lease with id {id} not found.");

        Bike bike = await bikeRepository.GetById(dto.BikeId)
                    ?? throw new ArgumentException($"Bike with id {dto.BikeId} not found.");

        Renter renter = await renterRepository.GetById(dto.RenterId)
                        ?? throw new ArgumentException($"Renter with id {dto.RenterId} not found.");

        Lease entityToUpdate = dto.ToEntity(bike, renter);
        entityToUpdate.Id = id;
        await leaseRepository.Update(entityToUpdate);
        Lease? updatedEntity = await leaseRepository.GetById(id);
        return updatedEntity!.ToDto();
    }

    public async Task<bool> Delete(int id)
    {
        Lease? entity = await leaseRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }

        await leaseRepository.Delete(entity);
        return true;
    }
}