using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;
using BikeRental.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Repositories;

public sealed class BikeRepository(ApplicationDbContext dbContext) : IBikeRepository
{
    public async Task<List<Bike>> GetAll()
    {
        return await dbContext.Bikes
            .ToListAsync();
    }

    public async Task<Bike?> GetById(int id)
    {
        return await dbContext.Bikes
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> Add(Bike entity)
    {
        dbContext.Bikes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Bike entity)
    {
        if (dbContext.Bikes.Local.All(e => e.Id != entity.Id))
        {
            dbContext.Bikes.Attach(entity);
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Bike entity)
    {
        dbContext.Bikes.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}

