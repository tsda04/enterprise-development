using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappings;
using BikeRental.Domain.Interfaces;

namespace BikeRental.Application.Services;

/// <summary>
/// Application-сервис для работы с арендаторами. Инкапсулирует бизнес-логику и доступ к репозиторию.
/// На текущем этапе является тонкой обёрткой над IRenterRepository.
/// </summary>
public sealed class RenterService(IRenterRepository renterRepository) : IRenterService
{
    public async Task<IEnumerable<RenterDto>> GetAll()
    {
        return (await renterRepository.GetAll()).
            Select(r => r.ToDto());
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
            var createdEntity = await renterRepository.GetById(id);
            if (createdEntity != null)
            {
                return createdEntity.ToDto();
            }
        }
        throw new InvalidOperationException("Failed to create entity.");
    }

    public async Task<RenterDto> Update(int id, RenterCreateUpdateDto dto)
    {
        var createdEntity = await renterRepository.GetById(id)
        ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
        
        var entityToUpdate = dto.ToEntity();
        entityToUpdate.Id = id;
        await renterRepository.Update(entityToUpdate);
        var updatedEntity = await renterRepository.GetById(id);
        return updatedEntity!.ToDto();
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await renterRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }
        await renterRepository.Delete(entity);
        return true;
    }
}

