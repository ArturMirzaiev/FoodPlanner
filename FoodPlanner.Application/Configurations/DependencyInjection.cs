using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Application.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(currentAssembly));
        
        services.AddAutoMapper(cfg => {}, currentAssembly);

        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddFluentValidationAutoValidation();

        return services;
    }
}