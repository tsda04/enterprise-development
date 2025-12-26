using BikeRental.Application.Contracts.Dtos;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Mappings;

internal static class BikeModelMappings
{
    public static BikeModelDto ToDto(this BikeModel entity)
    {
        return new BikeModelDto
        {
            Id = entity.Id,
            Type = entity.Type,
            WheelSize = entity.WheelSize,
            MaxСyclistWeight = entity.MaxCyclistWeight,
            Weight = entity.Weight,
            BrakeType = entity.BrakeType,
            YearOfManufacture = entity.YearOfManufacture,
            RentPrice = entity.RentPrice
        };
    }

    public static BikeModel ToEntity(this BikeModelCreateUpdateDto dto)
    {
        return new BikeModel
        {
            Type = dto.Type,
            WheelSize = dto.WheelSize,
            MaxCyclistWeight = dto.MaxCyclistWeight,
            Weight = dto.Weight,
            BrakeType = dto.BrakeType,
            YearOfManufacture = dto.YearOfManufacture,
            RentPrice = dto.RentPrice
        };
    }
}