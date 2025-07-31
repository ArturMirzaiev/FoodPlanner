using AutoMapper;
using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Services.Contracts;
using MediatR;

namespace FoodPlanner.Application.Ingredients.Features.Create;

public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, IngredientDto>
{
    private readonly IIngredientService _ingredientService;
    private readonly IMapper _mapper;

    public CreateIngredientCommandHandler(IIngredientService ingredientService, IMapper mapper)
    {
        _ingredientService = ingredientService;
        _mapper = mapper;
    }

    public async Task<IngredientDto> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        var creatingIngredientDto = _mapper.Map<CreateIngredientDto>(request);
        
        return await _ingredientService.AddAsync(creatingIngredientDto, cancellationToken);
    }
}