using FoodPlanner.Application.Authentication.Services.Contracts;
using FoodPlanner.Application.Shared.Services;
using FoodPlanner.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Infrastructure.DI.Extensions;

internal static class ServiceRegistrationExtensions
{
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // Authentication feature services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserService, UserService>();
        
        // Shared/common services used across multiple features
        services.AddScoped<IUserContextService, UserContextService>();
        
        // System services
        services.AddHttpContextAccessor();

        return services;
    }
}