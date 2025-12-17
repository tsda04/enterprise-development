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
        // Установить наименование таблицы
        builder.ToTable("Bikes");

        // Первичный ключ
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        // Серийный номер велосипеда
        builder.Property(b => b.SerialNumber)
            .IsRequired()
            .HasMaxLength(64);

        // Цвет
        builder.Property(b => b.Color)
            .IsRequired()
            .HasMaxLength(32);

        // Внешний ключ на модель велосипеда
        builder.Property(b => b.ModelId)
            .IsRequired();

        // Навигация на модель велосипеда
        builder.HasOne(b => b.Model)
            .WithMany()
            .HasForeignKey(b => b.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        // Уникальный индекс по серийному номеру
        builder.HasIndex(b => b.SerialNumber)
            .IsUnique();
    }
}
