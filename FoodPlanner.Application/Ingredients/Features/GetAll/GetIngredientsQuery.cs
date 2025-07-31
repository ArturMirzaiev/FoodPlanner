using FoodPlanner.Application.Ingredients.Dtos;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.GetAll;

public class GetIngredientsQuery : IRequest<List<IngredientDto>?>
{
    
}