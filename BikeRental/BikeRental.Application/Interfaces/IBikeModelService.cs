using BikeRental.Application.Contracts.Dtos;

namespace BikeRental.Application.Interfaces;

public interface IBikeModelService
{
    /// <summary>
    /// Returns all bike models.
    /// </summary>
    public Task<IEnumerable<BikeModelDto>> GetAll();

    /// <summary>
    /// Returns a bike model by id.
    /// </summary>
    public Task<BikeModelDto?> GetById(int id);

    /// <summary>
    /// Creates a new bike model.
    /// </summary>
    public Task<BikeModelDto> Create(BikeModelCreateUpdateDto dto);

    /// <summary>
    /// Updates an existing bike model.
    /// </summary>
    public Task<BikeModelDto> Update(int id, BikeModelCreateUpdateDto dto);

    /// <summary>
    /// Deletes a bike model.
    /// </summary>
    public Task<bool> Delete(int id);

}