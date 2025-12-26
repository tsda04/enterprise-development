namespace BikeRental.Domain.Interfaces;

/// <summary>
///     Generic repository interface that defines basic CRUD operations.
/// </summary>
public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    ///     Returns all entities.
    /// </summary>
    public Task<List<TEntity>> GetAll();

    /// <summary>
    ///     Returns entity by id.
    /// </summary>
    public Task<TEntity?> GetById(int id);

    /// <summary>
    ///     Adds a new entity and returns its generated id.
    /// </summary>
    public Task<int> Add(TEntity entity);

    /// <summary>
    ///     Updates existing entity.
    /// </summary>
    public Task Update(TEntity entity);

    /// <summary>
    ///     Deletes existing entity.
    /// </summary>
    public Task Delete(TEntity entity);
}