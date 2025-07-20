using FoodPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class MealDishConfiguration : IEntityTypeConfiguration<MealDish>
{
    public void Configure(EntityTypeBuilder<MealDish> builder)
    {
        builder.HasKey(md => new { md.MealId, md.DishId });

        builder.HasOne(md => md.Meal)
            .WithMany(m => m.MealDishes)
            .HasForeignKey(md => md.MealId);

        builder.HasOne(md => md.Dish)
            .WithMany(d => d.MealDishes)
            .HasForeignKey(md => md.DishId);

        builder.ToTable("MealDishes");
    }
}
