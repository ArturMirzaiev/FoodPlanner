using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Features.Login;
using FoodPlanner.Application.Authentication.Features.Register;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Core.Constants;
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

        return Ok(ApiResponse<LoginResponseDto>
            .SuccessResponse(result, AuthenticationMessages.LoginSuccessfull));
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(ApiResponse<RegisterResponseDto>
            .SuccessResponse(result, AuthenticationMessages.RegistrationSuccessfull));
    }
}