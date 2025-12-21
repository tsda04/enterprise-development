using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Infrastructure.Database.Configurations;

/// <summary>
/// Конфигурация сущности "Bike"
/// </summary>
public class BikeConfiguration : IEntityTypeConfiguration<Bike>
{
    /// <summary>
    /// Настройка сущности "Bike"
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Bike> builder)
    {
        builder.ToTable("Bikes");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.SerialNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(b => b.Color)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(b => b.ModelId)
            .IsRequired();

        builder.HasOne(b => b.Model)
            .WithMany()
            .HasForeignKey(b => b.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.SerialNumber)
            .IsUnique();
    }
}