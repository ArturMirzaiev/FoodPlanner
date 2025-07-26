using Microsoft.Extensions.DependencyInjection;

namespace FoodPlanner.Features;

public static class IngredientsFeatureExtensions
{
    public static IServiceCollection AddIngredientsFeature(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(IngredientsFeatureExtensions).Assembly);
        
        return services;
    }
}