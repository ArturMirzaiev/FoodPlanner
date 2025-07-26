using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Interfaces;
using FoodPlanner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodPlanner.Infrastructure.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IngredientRepository> _logger;

    public IngredientRepository(ApplicationDbContext context, ILogger<IngredientRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Ingredient>> GetAvailableIngredientsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Ingredients
            .Where(i => i.UserId == null || i.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ingredient?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == id && (i.UserId == null || i.UserId == userId), cancellationToken);
    }

    public async Task AddAsync(Ingredient ingredient, CancellationToken cancellationToken = default)
    {
        await _context.Ingredients.AddAsync(ingredient, cancellationToken);
    }

    public void Remove(Ingredient ingredient)
    {
        _context.Ingredients.Remove(ingredient);
    }


    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
