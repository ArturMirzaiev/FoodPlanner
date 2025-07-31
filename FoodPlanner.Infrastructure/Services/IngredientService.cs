using AutoMapper;
using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Services.Contracts;
using FoodPlanner.Application.Shared.Services;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Exceptions;
using FoodPlanner.Domain.Interfaces;

namespace FoodPlanner.Infrastructure.Services;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;
    
    public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper, IUserContextService userContextService)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<List<IngredientDto>> GetCurrentUserIngredientsAsync(CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.GetUserIdOrThrow();

        var ingredients = await _ingredientRepository.GetByFilterAsync(
            i => i.UserId == userId || i.UserId == null,
            trackChanges: false, cancellationToken);
        
        return _mapper.Map<List<IngredientDto>>(ingredients);
    }

    public async Task<IngredientDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Ingredient id cannot be empty.", nameof(id));
        
        var ingredient = await _ingredientRepository.GetByIdAsync(id, trackChanges: false, cancellationToken);

        if (ingredient is null)
            throw IngredientException.NotFound(id);

        var userId = _userContextService.GetUserIdOrThrow();

        if (ingredient.UserId is not null && ingredient.UserId != userId)
            throw IngredientException.NotOwned(id);

        return _mapper.Map<IngredientDto>(ingredient);
    }

    public async Task<IngredientDto> AddAsync(CreateIngredientDto createIngredientDto, CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.GetUserIdOrThrow();
        
        var ingredient = _mapper.Map<Ingredient>(createIngredientDto);
        ingredient.UserId = userId;

        await _ingredientRepository.CreateAsync(ingredient, cancellationToken);
        await _ingredientRepository.SaveAsync(cancellationToken);
        
        return _mapper.Map<IngredientDto>(ingredient);
    }

    public async Task<IngredientDto> UpdateAsync(UpdateIngredientDto updateIngredientDto, CancellationToken cancellationToken = default)
    {
        if (updateIngredientDto.Id == Guid.Empty)
            throw new ArgumentException("Ingredient id cannot be empty.", nameof(updateIngredientDto.Id));

        var ingredient = await _ingredientRepository.GetByIdAsync(updateIngredientDto.Id, trackChanges: true, cancellationToken);

        if (ingredient is null)
            throw IngredientException.NotFound(updateIngredientDto.Id);
        
        var userId = _userContextService.GetUserIdOrThrow();

        if (ingredient.UserId is null)
            throw IngredientException.IsReadOnly(updateIngredientDto.Id);

        if (userId != ingredient.UserId)
            throw IngredientException.NotOwned(ingredient.Id);
        
        _mapper.Map(updateIngredientDto, ingredient);
        await _ingredientRepository.SaveAsync(cancellationToken);

        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
        return ingredientDto;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Ingredient id cannot be empty.", nameof(id));
        
        var ingredient = await _ingredientRepository.GetByIdAsync(id, true, cancellationToken);
        
        if (ingredient is null)
            throw IngredientException.NotFound(id);
        
        var userId = _userContextService.GetUserIdOrThrow();

        if (ingredient.UserId is null)
            throw IngredientException.IsReadOnly(id);

        if (userId != ingredient.UserId)
            throw IngredientException.NotOwned(ingredient.Id);
        
        _ingredientRepository.Remove(ingredient);
        await _ingredientRepository.SaveAsync(cancellationToken);
    }
}