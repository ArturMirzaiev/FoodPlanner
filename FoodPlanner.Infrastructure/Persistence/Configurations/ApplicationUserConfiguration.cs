using FoodPlanner.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodPlanner.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AspNetUsers");

        var userId = Guid.Parse("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7");

        var hasher = new PasswordHasher<ApplicationUser>();
        var user = new ApplicationUser
        {
            Id = userId,
            UserName = "aresmi",
            NormalizedUserName = "ARESMI",
            Email = "aresmi@example.com",
            NormalizedEmail = "ARESMI@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };

        user.PasswordHash = hasher.HashPassword(user, "String12345!");

        builder.HasData(user);
    }
}