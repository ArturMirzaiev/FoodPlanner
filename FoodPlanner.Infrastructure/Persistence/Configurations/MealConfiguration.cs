using FoodPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Time);

        builder.HasOne(m => m.MenuDay)
            .WithMany(d => d.Meals)
            .HasForeignKey(m => m.MenuDayId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.MealDishes)
            .WithOne(md => md.Meal)
            .HasForeignKey(md => md.MealId);
    }
}