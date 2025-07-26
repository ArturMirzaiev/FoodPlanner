using AutoMapper;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Features.Authentication.Commands;

namespace FoodPlanner.Features.Authentication.Mappings;

public class AuthenticationMapperProfile : Profile
{
    public AuthenticationMapperProfile()
    {
        CreateMap<LoginUserCommand, LoginDto>();
        
        CreateMap<RegisterUserCommand, RegisterDto>();
    }
}