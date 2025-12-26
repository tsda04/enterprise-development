using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Application.Interfaces;

/// <summary>
///     Service for managing bikes.
/// </summary>
public interface IBikeService
{
    public Task<IEnumerable<BikeDto>> GetAll();
    public Task<BikeDto?> GetById(int id);
    public Task<BikeDto> Create(BikeCreateUpdateDto dto);
    public Task<BikeDto> Update(int id, BikeCreateUpdateDto dto);
    public Task<bool> Delete(int id);
}