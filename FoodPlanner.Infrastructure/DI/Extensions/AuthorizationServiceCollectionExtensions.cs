using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Infrastructure.DI.Extensions;

public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // empty
        });

        return services;
    }   
}