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
            .Include(l => l.Model)
            .ToListAsync();
    }

    public async Task<Bike?> GetById(int id)
    {
        return await dbContext.Bikes
            .Include(l => l.Model)
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
        Bike existing = await dbContext.Bikes.FindAsync(entity.Id)
                        ?? throw new KeyNotFoundException($"Bike with id {entity.Id} not found.");

        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Bike entity)
    {
        dbContext.Bikes.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}