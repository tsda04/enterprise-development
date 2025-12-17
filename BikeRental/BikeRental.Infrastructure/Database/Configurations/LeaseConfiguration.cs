using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRental.Infrastructure.Database.Configurations;

/// <summary>
/// Конфигурация сущности "Lease"
/// </summary>
public class LeaseConfiguration : IEntityTypeConfiguration<Lease>
{
    /// <summary>
    /// Настройка сущности "Lease"
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Lease> builder)
    {
        // Установить наименование таблицы
        builder.ToTable("Leases");

        // Первичный ключ
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd();

        // Связь с арендатором
        builder.HasOne(l => l.Renter)
            .WithMany() // коллекция договоров у Renter пока не определена
            .IsRequired();

        // Связь с велосипедом
        builder.HasOne(l => l.Bike)
            .WithMany() // коллекция договоров у Bike пока не определена
            .IsRequired();

        // Дата и время начала аренды
        builder.Property(l => l.RentalStartTime)
            .IsRequired();

        // Продолжительность аренды
        builder.Property(l => l.RentalDuration)
            .IsRequired();
    }
}

