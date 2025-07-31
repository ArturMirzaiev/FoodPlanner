using FoodPlanner.Application.Ingredients.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.Delete;

public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand, Unit>
{
    private readonly IIngredientService _ingredientService;

    public DeleteIngredientCommandHandler(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    public async Task<Unit> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        await _ingredientService.DeleteAsync(request.Id, cancellationToken);
        
        return Unit.Value;
    }
}