using FoodPlanner.Application.Dtos;
using FoodPlanner.Domain.Core.Common;
using MediatR;

namespace FoodPlanner.Features.Authentication.Commands;

public class RegisterUserCommand : IRequest<Result<RegisterResponseDto>>
{   
    public string Username { get; set; }
    public string Password { get; set; }
}

