using FoodPlanner.Application.Dtos;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Responses;
using FoodPlanner.Features.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(ApiResponse<LoginResponseDto>.FailureResponse(result.Errors, "Login failed"));

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(result.Value!, "Login successful"));
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<RegisterResponseDto>.FailureResponse(result.Errors, "Registration failed."));
        }
        
        return Ok(ApiResponse<RegisterResponseDto>.SuccessResponse(result.Value!, "Registration successful."));
    }
}