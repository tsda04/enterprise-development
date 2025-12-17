using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;
using BikeRental.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с моделями велосипедов.
/// </summary>
public sealed class BikeModelRepository(ApplicationDbContext dbContext) : IBikeModelRepository
{
    public async Task<List<BikeModel>> GetAll()
    {
        return await dbContext.BikeModels
            .ToListAsync();
    }

    public async Task<BikeModel?> GetById(int id)
    {
        return await dbContext.BikeModels
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> Add(BikeModel entity)
    {
        dbContext.BikeModels.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(BikeModel entity)
    {
        if (dbContext.BikeModels.Local.All(e => e.Id != entity.Id))
        {
            dbContext.BikeModels.Attach(entity);
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(BikeModel entity)
    {
        dbContext.BikeModels.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
