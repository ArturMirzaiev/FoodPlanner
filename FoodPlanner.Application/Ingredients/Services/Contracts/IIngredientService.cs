using FoodPlanner.Application.Ingredients.Dtos;

namespace FoodPlanner.Application.Ingredients.Services.Contracts;

public interface IIngredientService
{
    Task<List<IngredientDto>> GetCurrentUserIngredientsAsync(CancellationToken cancellationToken = default);
    Task<IngredientDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IngredientDto> AddAsync(CreateIngredientDto createIngredientDto, CancellationToken cancellationToken = default);
    Task<IngredientDto> UpdateAsync(UpdateIngredientDto updateIngredientDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}   
