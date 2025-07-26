using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Domain.Interfaces;

public interface IIngredientRepository
{
    Task<List<Ingredient>> GetAvailableIngredientsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Ingredient?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Ingredient ingredient, CancellationToken cancellationToken = default);
    void Remove(Ingredient ingredient);
    Task SaveAsync(CancellationToken cancellationToken = default);
}

