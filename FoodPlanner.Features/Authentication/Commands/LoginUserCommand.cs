using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Responses;
using MediatR;

namespace FoodPlanner.Features.Authentication.Commands;

public class LoginUserCommand : IRequest<Result<LoginResponseDto>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}