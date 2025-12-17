using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappings;
using BikeRental.Domain.Interfaces;

namespace BikeRental.Application.Services;

/// <summary>
/// Application-сервис для работы с велосипедами. Инкапсулирует бизнес-логику и доступ к репозиторию.
/// На текущем этапе является тонкой обёрткой над IBikeRepository.
/// </summary>
public sealed class BikeService(IBikeRepository bikeRepository) : IBikeService
{
    public async Task<IEnumerable<BikeDto>> GetAll()
    {
        return (await bikeRepository.GetAll()).
            Select(b => b.ToDto());
    }

    public async Task<BikeDto?> GetById(int id)
    {
        return (await bikeRepository.GetById(id))?.ToDto();
    }

    public async Task<BikeDto> Create(BikeCreateUpdateDto dto)
    {
        var id = await bikeRepository.Add(dto.ToEntity());
        if (id > 0)
        {
            var createdEntity = await bikeRepository.GetById(id);
            if (createdEntity != null)
            {
                return createdEntity.ToDto();
            }
        }
        throw new InvalidOperationException("Failed to create entity.");
    }

    public async Task<BikeDto> Update(int id, BikeCreateUpdateDto dto)
    {
        var createdEntity = await bikeRepository.GetById(id);
        if (createdEntity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        var entityToUpdate = dto.ToEntity();
        entityToUpdate.Id = id;
        await bikeRepository.Update(entityToUpdate);
        var updatedEntity = await bikeRepository.GetById(id);
        return updatedEntity!.ToDto();
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await bikeRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }
        await bikeRepository.Delete(entity);
        return true;
    }
}
