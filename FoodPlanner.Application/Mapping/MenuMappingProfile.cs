using AutoMapper;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Mapping;

public class MenuMappingProfile : Profile
{
    public MenuMappingProfile()
    {
        CreateMap<Ingredient, IngredientDto>();

        CreateMap<DishIngredient, DishIngredientDto>()
            .ForMember(dest => dest.Ingredient, opt => opt.MapFrom(src => src.Ingredient));

        CreateMap<Dish, DishDto>();

        CreateMap<MealDish, MealDishDto>()
            .ForMember(dest => dest.Dish, opt => opt.MapFrom(src => src.Dish));
    
        CreateMap<Meal, MealDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.MealDishes));

        CreateMap<MenuDay, MenuDayDto>()
            .ForMember(dest => dest.DayNumber, opt => opt.MapFrom(src => src.Order));

        CreateMap<Menu, MenuDto>();

    }
}
