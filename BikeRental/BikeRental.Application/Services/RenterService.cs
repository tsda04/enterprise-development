using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappings;
using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
///     Application-сервис для работы с арендаторами. Инкапсулирует бизнес-логику и доступ к репозиторию.
///     На текущем этапе является тонкой обёрткой над IRenterRepository.
/// </summary>
public sealed class RenterService(IRenterRepository renterRepository) : IRenterService
{
    public async Task<IEnumerable<RenterDto>> GetAll()
    {
        return (await renterRepository.GetAll()).Select(r => r.ToDto());
    }

    public async Task<RenterDto?> GetById(int id)
    {
        return (await renterRepository.GetById(id))?.ToDto();
    }

    public async Task<RenterDto> Create(RenterCreateUpdateDto dto)
    {
        var id = await renterRepository.Add(dto.ToEntity());
        if (id > 0)
        {
            Renter? createdEntity = await renterRepository.GetById(id);
            if (createdEntity != null)
            {
                return createdEntity.ToDto();
            }
        }

        throw new InvalidOperationException("Failed to create entity.");
    }

    public async Task<RenterDto> Update(int id, RenterCreateUpdateDto dto)
    {
        _ = await renterRepository.GetById(id)
            ?? throw new KeyNotFoundException($"Entity with id {id} not found.");

        Renter entityToUpdate = dto.ToEntity();
        entityToUpdate.Id = id;
        await renterRepository.Update(entityToUpdate);
        Renter? updatedEntity = await renterRepository.GetById(id);
        return updatedEntity!.ToDto();
    }

    public async Task<bool> Delete(int id)
    {
        Renter? entity = await renterRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }

        await renterRepository.Delete(entity);
        return true;
    }
}