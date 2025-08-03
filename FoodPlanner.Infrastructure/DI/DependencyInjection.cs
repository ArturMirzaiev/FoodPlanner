using System.Reflection;
using FoodPlanner.Infrastructure.DI.Extensions;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplicationDbContext(configuration) 
            .AddIdentityInfrastructure()
            .AddJwtAuthentication(configuration)
            .AddAuthorizationPolicies()
            .AddSwaggerDocumentation()
            .AddAppServices();

        return services;
    }

}