using AutoMapper;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Features.Login;
using FoodPlanner.Application.Authentication.Features.Register;

namespace FoodPlanner.Application.Authentication.Features.Configurations;

public class AuthenticationMapperProfile : Profile
{
    public AuthenticationMapperProfile()
    {
        CreateMap<LoginUserCommand, LoginDto>();
        
        CreateMap<RegisterUserCommand, RegisterDto>();
    }
}