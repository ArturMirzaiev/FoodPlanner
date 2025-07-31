using FoodPlanner.Application.Authentication.Dtos;
using MediatR;

namespace FoodPlanner.Application.Authentication.Features.Register;

public class RegisterUserCommand : IRequest<RegisterResponseDto>
{   
    public string Username { get; set; }
    public string Password { get; set; }
}

