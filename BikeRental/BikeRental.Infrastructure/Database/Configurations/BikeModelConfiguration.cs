using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Infrastructure.Database.Configurations;

/// <summary>
/// Конфигурация сущности "BikeModel"
/// </summary>
public class BikeModelConfiguration : IEntityTypeConfiguration<BikeModel>
{
    /// <summary>
    /// Настройка сущности "BikeModel"
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<BikeModel> builder)
    {
        builder.ToTable("BikeModels");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Type)
            .IsRequired();

        builder.Property(b => b.WheelSize)
            .IsRequired();

        builder.Property(b => b.MaxCyclistWeight)
            .IsRequired();

        builder.Property(b => b.Weight)
            .IsRequired();

        builder.Property(b => b.BrakeType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.YearOfManufacture)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(b => b.RentPrice)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        // Индексы для типичных сценариев выборки

        // Индекс по типу велосипеда
        builder.HasIndex(b => b.Type);
        // Индекс по комбинации типа велосипеда и размера колеса
        builder.HasIndex(b => new { b.Type, b.WheelSize });
    }
}