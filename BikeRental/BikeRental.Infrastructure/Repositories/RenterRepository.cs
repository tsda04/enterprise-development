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
        if (dbContext.Renters.Local.All(e => e.Id != entity.Id))
        {
            dbContext.Renters.Attach(entity);
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Renter entity)
    {
        dbContext.Renters.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}

