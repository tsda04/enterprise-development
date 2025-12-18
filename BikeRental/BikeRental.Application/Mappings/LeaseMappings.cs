using BikeRental.Application.Contracts.Dtos;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Mappings;

internal static class LeaseMappings
{
    public static LeaseDto ToDto(this Lease entity)
    {
        return new LeaseDto
        {
            Id = entity.Id,
            BikeId = entity.Bike.Id,
            RenterId = entity.Renter.Id,
            RentalStartTime = entity.RentalStartTime,
            RentalDuration = entity.RentalDuration
        };
    }
    
    public static Lease ToEntity(this LeaseCreateUpdateDto dto, Bike bike, Renter renter)
    {
        return new Lease
        {
            BikeId =  bike.Id,
            RenterId =  renter.Id,
            Bike = bike,
            Renter = renter,
            RentalStartTime = dto.RentalStartTime,
            RentalDuration = dto.RentalDuration
        };
    }
}