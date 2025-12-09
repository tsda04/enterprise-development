using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Application.Interfaces;

/// <summary>
/// Service for managing renters.
/// </summary>
public interface IRenterService
{
    public Task<IEnumerable<RenterDto>> GetAll();
    public Task<RenterDto?> GetById(int id);
    public Task<RenterDto> Create(RenterCreateUpdateDto dto);
    public Task<RenterDto> Update(int id, RenterCreateUpdateDto dto);
    public Task<bool> Delete(int id);
}