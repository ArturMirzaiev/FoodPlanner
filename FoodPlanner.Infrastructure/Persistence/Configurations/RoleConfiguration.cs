using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.ToTable("AspNetRoles"); 
        
        builder.HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("57454260-b650-4114-864f-9625ced0b947"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("5c15241a-44e5-4ece-9eca-f5d832af5dad"),
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }
}