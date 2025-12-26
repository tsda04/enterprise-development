using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappings;
using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
///     Application-сервис для работы с велосипедами. Инкапсулирует бизнес-логику и доступ к репозиторию.
///     На текущем этапе является тонкой обёрткой над IBikeRepository.
/// </summary>
public sealed class BikeService(IBikeRepository bikeRepository, IBikeModelRepository modelRepository) : IBikeService
{
    public async Task<IEnumerable<BikeDto>> GetAll()
    {
        return (await bikeRepository.GetAll()).Select(b => b.ToDto());
    }

    public async Task<BikeDto?> GetById(int id)
    {
        return (await bikeRepository.GetById(id))?.ToDto();
    }

    public async Task<BikeDto> Create(BikeCreateUpdateDto dto)
    {
        BikeModel model = await modelRepository.GetById(dto.ModelId)
                          ?? throw new ArgumentException($"Model with id {dto.ModelId} not found.");

        var id = await bikeRepository.Add(dto.ToEntity(model));

        if (id > 0)
        {
            Bike? createdEntity = await bikeRepository.GetById(id);
            if (createdEntity != null)
            {
                return createdEntity.ToDto();
            }
        }

        throw new InvalidOperationException("Failed to create entity.");
    }

    public async Task<BikeDto> Update(int id, BikeCreateUpdateDto dto)
    {
        _ = await bikeRepository.GetById(id)
            ?? throw new KeyNotFoundException($"Bike with id {id} not found.");

        BikeModel model = await modelRepository.GetById(dto.ModelId)
                          ?? throw new ArgumentException($"Model with id {dto.ModelId} not found.");

        Bike entityToUpdate = dto.ToEntity(model);
        entityToUpdate.Id = id;
        await bikeRepository.Update(entityToUpdate);
        Bike? updatedEntity = await bikeRepository.GetById(id);
        return updatedEntity!.ToDto();
    }

    public async Task<bool> Delete(int id)
    {
        Bike? entity = await bikeRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }

        await bikeRepository.Delete(entity);
        return true;
    }
}