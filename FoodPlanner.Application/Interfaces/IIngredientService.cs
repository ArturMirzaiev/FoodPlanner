using FoodPlanner.Application.Dtos;

namespace FoodPlanner.Application.Interfaces;

public interface IIngredientService
{
    Task<List<IngredientDto>> GetAvailableIngredientsAsync(Guid userId, CancellationToken cancellationToken);
    Task<IngredientDto?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<IngredientDto> AddAsync(CreateIngredientDto createIngredientDto, Guid userId, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateIngredientDto updateIngredientDto, Guid userId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken);
}
