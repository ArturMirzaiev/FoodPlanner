using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.GetById;

public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, IngredientDto>
{
    private readonly IIngredientService _ingredientService;

    public GetIngredientByIdQueryHandler(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    public async Task<IngredientDto> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _ingredientService.GetByIdAsync(request.Id, cancellationToken);
    }
}