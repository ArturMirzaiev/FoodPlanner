using System.Linq.Expressions;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Domain.Interfaces;

public interface IIngredientRepository
{
    Task<List<Ingredient>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<Ingredient?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<List<Ingredient>> GetByFilterAsync(Expression<Func<Ingredient, bool>> predicate,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
    Task CreateAsync(Ingredient ingredient, CancellationToken cancellationToken = default);
    void Remove(Ingredient ingredient);
    void Update(Ingredient ingredient);
    
    // In the future it is necessary to take it out in IUnitOfWork
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}

