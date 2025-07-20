using FoodPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.IsPurchased)
            .IsRequired();

        builder.HasMany(d => d.MealDishes)
            .WithOne(md => md.Dish)
            .HasForeignKey(md => md.DishId);

        builder.HasMany(d => d.Ingredients)
            .WithOne(di => di.Dish)
            .HasForeignKey(di => di.DishId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
