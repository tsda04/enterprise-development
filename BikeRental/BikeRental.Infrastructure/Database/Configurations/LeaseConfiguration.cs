using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Infrastructure.Database.Configurations;

/// <summary>
///     Конфигурация сущности "Lease"
/// </summary>
public class LeaseConfiguration : IEntityTypeConfiguration<Lease>
{
    /// <summary>
    ///     Настройка сущности "Lease"
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Lease> builder)
    {
        builder.ToTable("Leases");

        // Первичный ключ
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.RenterId)
            .IsRequired();

        // Связь с арендатором
        builder.HasOne(l => l.Renter)
            .WithMany()
            .HasForeignKey(l => l.RenterId)
            .IsRequired();


        builder.Property(l => l.BikeId)
            .IsRequired();

        // Связь с велосипедом
        builder.HasOne(l => l.Bike)
            .WithMany()
            .HasForeignKey(l => l.BikeId)
            .IsRequired();

        builder.Property(l => l.RentalStartTime)
            .IsRequired();

        builder.Property(l => l.RentalDuration)
            .IsRequired();
    }
}