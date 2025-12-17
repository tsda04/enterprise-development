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
        // Установить наименование таблицы
        builder.ToTable("BikeModels");

        // Первичный ключ
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        // Тип велосипеда (enum BikeType) — храним как int по умолчанию
        builder.Property(b => b.Type)
            .IsRequired();

        // Размер колеса
        builder.Property(b => b.WheelSize)
            .IsRequired();

        // Максимальный вес велосипедиста
        builder.Property(b => b.MaxCyclistWeight)
            .IsRequired();

        // Вес велосипеда
        builder.Property(b => b.Weight)
            .IsRequired();

        // Тип тормозной системы
        builder.Property(b => b.BrakeType)
            .IsRequired()
            .HasMaxLength(50);

        // Год выпуска модели (строка из 4 символов)
        builder.Property(b => b.YearOfManufacture)
            .IsRequired()
            .HasMaxLength(4);

        // Стоимость аренды в час
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