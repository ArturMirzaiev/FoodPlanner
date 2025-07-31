using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.Delete;

public class DeleteIngredientCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public DeleteIngredientCommand(Guid id)
    {
        Id = id;
    }
}