using FoodPlanner.Application.Ingredients.Dtos;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.Create;

public class CreateIngredientCommand : IRequest<IngredientDto>
{
    public string Name { get; set; }
}