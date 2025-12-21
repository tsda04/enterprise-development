using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;
using BikeRental.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Repositories;

public sealed class RenterRepository(ApplicationDbContext dbContext) : IRenterRepository
{
    public async Task<List<Renter>> GetAll()
    {
        return await dbContext.Renters
            .ToListAsync();
    }

    public async Task<Renter?> GetById(int id)
    {
        return await dbContext.Renters
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> Add(Renter entity)
    {
        dbContext.Renters.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Renter entity)
    {
        Renter existing = await dbContext.Renters.FindAsync(entity.Id)
                          ?? throw new KeyNotFoundException($"Renter with id {entity.Id} not found.");

        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Renter entity)
    {
        dbContext.Renters.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}