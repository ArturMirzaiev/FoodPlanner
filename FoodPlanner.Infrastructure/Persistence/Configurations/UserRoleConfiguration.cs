using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.ToTable("AspNetUserRoles");

        builder.HasData(
            new IdentityUserRole<Guid>
            {
                UserId = Guid.Parse("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"), // aresmi
                RoleId = Guid.Parse("5c15241a-44e5-4ece-9eca-f5d832af5dad")  // User
            }
        );
    }
}