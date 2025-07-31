using AutoMapper;
using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Features.Create;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Ingredients.Configurations;

public class IngredientMapperProfile : Profile
{
    public IngredientMapperProfile()
    {
        CreateMap<CreateIngredientCommand, CreateIngredientDto>();
        
        CreateMap<IngredientDto, Ingredient>()
            .ReverseMap();
        
        CreateMap<UpdateIngredientDto, Ingredient>()
            .ReverseMap();
        
        CreateMap<CreateIngredientDto, Ingredient>()
            .ReverseMap();
    }
}