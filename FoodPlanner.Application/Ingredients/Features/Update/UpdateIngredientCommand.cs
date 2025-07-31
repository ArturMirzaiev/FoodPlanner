using FoodPlanner.Application.Ingredients.Dtos;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.Update;

public class UpdateIngredientCommand : IRequest<IngredientDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}