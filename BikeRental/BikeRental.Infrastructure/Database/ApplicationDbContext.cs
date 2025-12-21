using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Database;

/// <summary>
///     Контекст базы данных приложения
/// </summary>
/// <param name="options"></param>
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>
    ///     Набор сущностей "BikeModel" (Модель велосипеда)
    /// </summary>
    public DbSet<BikeModel> BikeModels { get; set; }

    /// <summary>
    ///     Набор сущностей "Bike" (Велосипед)
    /// </summary>
    public DbSet<Bike> Bikes { get; set; }

    /// <summary>
    ///     Набор сущностей "Renter" (Арендатор)
    /// </summary>
    public DbSet<Renter> Renters { get; set; }

    /// <summary>
    ///     Набор сущностей "Lease" (Договор аренды)
    /// </summary>
    public DbSet<Lease> Leases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Применить конфигурации из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}