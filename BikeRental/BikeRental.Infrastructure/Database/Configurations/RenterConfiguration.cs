using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Infrastructure.Database.Configurations;

/// <summary>
///     Конфигурация сущности "Renter"
/// </summary>
public class RenterConfiguration : IEntityTypeConfiguration<Renter>
{
    /// <summary>
    ///     Настройка сущности "Renter"
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Renter> builder)
    {
        builder.ToTable("Renters");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.PhoneNumber)
            .IsRequired()
            .HasMaxLength(32);

        // Уникальный индекс по номеру телефона
        builder.HasIndex(r => r.PhoneNumber)
            .IsUnique();
    }
}