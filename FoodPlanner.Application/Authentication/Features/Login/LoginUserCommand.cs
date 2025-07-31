using FoodPlanner.Application.Authentication.Dtos;
using MediatR;

namespace FoodPlanner.Application.Authentication.Features.Login;

public class LoginUserCommand : IRequest<LoginResponseDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
}