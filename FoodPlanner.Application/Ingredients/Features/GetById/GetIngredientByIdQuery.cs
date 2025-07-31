using FoodPlanner.Application.Ingredients.Dtos;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.GetById;

public class GetIngredientByIdQuery : IRequest<IngredientDto>
{
    public Guid Id { get; set; }

    public GetIngredientByIdQuery(Guid id)
    {
        Id = id;
    }
}