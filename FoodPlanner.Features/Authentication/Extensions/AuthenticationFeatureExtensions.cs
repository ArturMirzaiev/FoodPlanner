using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Features.Authentication.Extensions;

public static class AuthenticationFeatureExtensions
{
    public static IServiceCollection AddAuthenticationFeature(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(AuthenticationFeatureExtensions).Assembly);
        
        return services;
    }
}