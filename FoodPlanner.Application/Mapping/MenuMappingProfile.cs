using AutoMapper;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Mapping;

public class MenuMappingProfile : Profile
{
    public MenuMappingProfile()
    {
        CreateMap<IngredientDto, Ingredient>()
            .ReverseMap();
        
        CreateMap<UpdateIngredientDto, Ingredient>()
            .ReverseMap();
        
        CreateMap<CreateIngredientDto, Ingredient>()
            .ReverseMap();
    }
}
