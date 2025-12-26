using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Application.Interfaces;

/// <summary>
///     Service for managing bike leases.
/// </summary>
public interface ILeaseService
{
    public Task<IEnumerable<LeaseDto>> GetAll();
    public Task<LeaseDto?> GetById(int id);
    public Task<LeaseDto> Create(LeaseCreateUpdateDto dto);
    public Task<LeaseDto> Update(int id, LeaseCreateUpdateDto dto);
    public Task<bool> Delete(int id);
}