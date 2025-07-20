using FoodPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class MenuDayConfiguration : IEntityTypeConfiguration<MenuDay>
{
    public void Configure(EntityTypeBuilder<MenuDay> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Order)
            .IsRequired();

        builder.HasOne(d => d.Menu)
            .WithMany(m => m.Days)
            .HasForeignKey(d => d.MenuId);

        builder.HasMany(d => d.Meals)
            .WithOne(m => m.MenuDay)
            .HasForeignKey(m => m.MenuDayId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}