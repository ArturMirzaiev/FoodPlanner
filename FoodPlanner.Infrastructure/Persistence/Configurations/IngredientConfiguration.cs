using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.HasOne(s => s.User)
            .WithMany(s => s.Ingredients)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        ConfigureSystemIngredients(builder);
        ConfigureUserIngredients(builder);
    }

    
    
    private void ConfigureSystemIngredients(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasData(
            new Ingredient
            {
                Id = new Guid("e0b781b9-1d53-41ac-a645-997ace341929"),
                Name = "Water"
            },
            new Ingredient
            {
                Id = new Guid("acc23aee-8bb6-417b-a24e-92f471cb531d"),
                Name = "Salt"
            }
        );
    }
    
    private void ConfigureUserIngredients(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasData(
            new Ingredient
            {
                Id = new Guid("7e36a395-6da1-4189-b072-2f702cc2eeed"),
                Name = "Chicken breast",
                UserId = new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7")
            },
            new Ingredient
            {
                Id = new Guid("e9f880ad-9920-47eb-97ac-69f4969bbd5e"),
                Name = "Rice",
                UserId = new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7")
            }
        );
    }
}