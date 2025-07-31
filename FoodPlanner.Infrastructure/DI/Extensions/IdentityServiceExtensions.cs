using FoodPlanner.Domain.Entities;
using FoodPlanner.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Infrastructure.DI.Extensions;

internal static class IdentityServiceExtensions
{
    internal static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}