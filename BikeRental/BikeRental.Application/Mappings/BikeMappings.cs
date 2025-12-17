using BikeRental.Application.Contracts.Dtos;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Mappings;

internal static class BikeMappings
{
    public static BikeDto ToDto(this Bike entity)
    {
        return new BikeDto
        {
            Id = entity.Id,
            SerialNumber = entity.SerialNumber,
            Color = entity.Color,
            ModelId = entity.ModelId
        };
    }
    
    public static Bike ToEntity(this BikeCreateUpdateDto dto)
    {
        return new Bike
        {
            SerialNumber = dto.SerialNumber,
            Color = dto.Color,
            ModelId = dto.ModelId
        };
    }
}