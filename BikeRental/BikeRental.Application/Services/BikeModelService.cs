using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappings;
using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
///     Application-сервис для работы с моделями велосипедов. Инкапсулирует бизнес-логику и доступ к репозиторию.
/// </summary>
public sealed class BikeModelService(IBikeModelRepository bikeModelRepository) : IBikeModelService
{
    public async Task<IEnumerable<BikeModelDto>> GetAll()
    {
        return (await bikeModelRepository.GetAll()).Select(bm => bm.ToDto());
    }

    public async Task<BikeModelDto?> GetById(int id)
    {
        return (await bikeModelRepository.GetById(id))?.ToDto();
    }

    public async Task<BikeModelDto> Create(BikeModelCreateUpdateDto dto)
    {
        var id = await bikeModelRepository.Add(dto.ToEntity());
        if (id > 0)
        {
            BikeModel? createdEntity = await bikeModelRepository.GetById(id);
            if (createdEntity != null)
            {
                return createdEntity.ToDto();
            }
        }

        throw new InvalidOperationException("Failed to create entity.");
    }

    public async Task<BikeModelDto> Update(int id, BikeModelCreateUpdateDto dto)
    {
        _ = await bikeModelRepository.GetById(id)
            ?? throw new KeyNotFoundException($"Entity with id {id} not found.");

        BikeModel entityToUpdate = dto.ToEntity();
        entityToUpdate.Id = id;
        await bikeModelRepository.Update(entityToUpdate);
        BikeModel? updatedEntity = await bikeModelRepository.GetById(id);
        return updatedEntity!.ToDto();
    }

    public async Task<bool> Delete(int id)
    {
        BikeModel? entity = await bikeModelRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }

        await bikeModelRepository.Delete(entity);
        return true;
    }
}