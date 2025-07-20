using FoodPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.StartFrom)
            .IsRequired();

        builder.HasMany(m => m.Days)
            .WithOne(d => d.Menu)
            .HasForeignKey(d => d.MenuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}