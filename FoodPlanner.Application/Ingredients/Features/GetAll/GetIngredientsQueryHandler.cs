using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.GetAll;

public class GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQuery, List<IngredientDto>?>
{
    private readonly IIngredientService _ingredientService;

    public GetIngredientsQueryHandler(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }
    
    public async Task<List<IngredientDto>?> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
    {
        return await _ingredientService.GetCurrentUserIngredientsAsync(cancellationToken);
    }
}