using FoodPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class DishIngredientConfiguration : IEntityTypeConfiguration<DishIngredient>
{
    public void Configure(EntityTypeBuilder<DishIngredient> builder)
    {
        builder.HasKey(di => new { di.DishId, di.IngredientId });

        builder.Property(di => di.Quantity)
            .IsRequired();

        builder.HasOne(di => di.Dish)
            .WithMany(d => d.Ingredients)
            .HasForeignKey(di => di.DishId);

        builder.HasOne(di => di.Ingredient)
            .WithMany()
            .HasForeignKey(di => di.IngredientId);
    }
}