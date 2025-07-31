using AutoMapper;
using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.Update;

public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, IngredientDto>
{
    private readonly IIngredientService _ingredientService;
    private readonly IMapper _mapper;

    public UpdateIngredientCommandHandler(IIngredientService ingredientService, IMapper mapper)
    {
        _ingredientService = ingredientService;
        _mapper = mapper;
    }

    public async Task<IngredientDto> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        var updatingIngredientDto = _mapper.Map<UpdateIngredientDto>(request);
        
        return await _ingredientService.UpdateAsync(updatingIngredientDto, cancellationToken);
    }
}