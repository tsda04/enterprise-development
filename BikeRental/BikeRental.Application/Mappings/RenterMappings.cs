using BikeRental.Application.Contracts.Dtos;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Mappings;

internal static class RenterMappings
{
    public static RenterDto ToDto(this Renter entity)
    {
        return new RenterDto
        {
            Id = entity.Id,
            FullName = entity.FullName,
            PhoneNumber = entity.PhoneNumber
        };
    }
    
    public static Renter ToEntity(this RenterCreateUpdateDto dto)
    {
        return new Renter
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber
        };
    }
}