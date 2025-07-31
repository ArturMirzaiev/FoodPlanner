using System.Linq.Expressions;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Interfaces;
using FoodPlanner.Infrastructure.Common.Extensions;
using FoodPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Infrastructure.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly ApplicationDbContext _context;
    public IngredientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ingredient>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default) =>
        await _context.Ingredients
            .AsTrackingIf(trackChanges)
            .ToListAsync(cancellationToken); 

    public async Task<Ingredient?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default) =>
        await _context.Ingredients
            .AsTrackingIf(trackChanges)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);


    public async Task CreateAsync(Ingredient ingredient, CancellationToken cancellationToken = default) =>
        await _context.Ingredients.AddAsync(ingredient, cancellationToken);

    public void Remove(Ingredient ingredient) =>
        _context.Ingredients.Remove(ingredient);

    // Update happens automatically if tracked
    public void Update(Ingredient ingredient) =>
        _context.Ingredients.Update(ingredient);

    public async Task<List<Ingredient>> GetByFilterAsync(Expression<Func<Ingredient, bool>> predicate, 
        bool trackChanges = false, CancellationToken cancellationToken = default) =>
            await _context.Ingredients
                .AsTrackingIf(trackChanges)
                .Where(predicate)
                .ToListAsync(cancellationToken);

    // in the future to take it out in IUnitOfWork
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
