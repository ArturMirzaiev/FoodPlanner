using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Enums;
using FoodPlanner.Domain.Responses;

namespace FoodPlanner.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserClaimsService _userClaimsService;

    public AuthService(IUserService userService, IJwtTokenService jwtTokenService, IUserClaimsService userClaimsService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _userClaimsService = userClaimsService;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await _userService.FindByNameAsync(dto.Username, cancellationToken);
        if (user == null)
            return Result<LoginResponseDto>.Failure("Invalid username or password");

        var isPasswordValid = await _userService.CheckPasswordAsync(user, dto.Password, cancellationToken);
        if (!isPasswordValid)
            return Result<LoginResponseDto>.Failure("Invalid username or password");
        
        var claims = await _userClaimsService.GetClaimsAsync(user, cancellationToken);
        if (claims is null || !claims.Any())
            return Result<LoginResponseDto>.Failure("User has no claims");
        
        var tokenResult = await _jwtTokenService.GenerateToken(claims);
        return Result<LoginResponseDto>.Success(tokenResult);
    }

    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Username,
            Email = dto.Email,
        };

        var createResult = await _userService.CreateUserAsync(user, dto.Password, cancellationToken);
        if (!createResult.IsSuccess)
            return Result<RegisterResponseDto>.Failure(createResult.Errors);

        if (!await _userService.RoleExistsAsync(dto.Role.ToString(), cancellationToken))
        {
            var roleCreateResult = await _userService.CreateRoleAsync(dto.Role.ToString(), cancellationToken);
            if (!roleCreateResult.IsSuccess)
                return Result<RegisterResponseDto>.Failure(roleCreateResult.Errors);
        }

        var addToRoleResult = await _userService.AddUserToRoleAsync(user, dto.Role.ToString(), cancellationToken);
        if (!addToRoleResult.IsSuccess)
            return Result<RegisterResponseDto>.Failure(addToRoleResult.Errors);

        var responseDto = new RegisterResponseDto
        {
            UserId = user.Id,
            Username = user.UserName,
            Role = dto.Role.ToString()
        };

        return Result<RegisterResponseDto>.Success(responseDto);
    }
}
