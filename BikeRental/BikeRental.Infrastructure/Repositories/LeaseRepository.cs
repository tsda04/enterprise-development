using BikeRental.Domain.Interfaces;
using BikeRental.Domain.Models;
using BikeRental.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Repositories;

public sealed class LeaseRepository(ApplicationDbContext dbContext) : ILeaseRepository
{
    public async Task<List<Lease>> GetAll()
    {
        return await dbContext.Leases
            .ToListAsync();
    }

    public async Task<Lease?> GetById(int id)
    {
        return await dbContext.Leases
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> Add(Lease entity)
    {
        dbContext.Leases.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Lease entity)
    {
        if (dbContext.Leases.Local.All(e => e.Id != entity.Id))
        {
            dbContext.Leases.Attach(entity);
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Lease entity)
    {
        dbContext.Leases.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
