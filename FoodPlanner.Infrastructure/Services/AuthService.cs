using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Services.Contracts;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Exceptions;

namespace FoodPlanner.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));
        
        var user = await _userService.FindByNameAsync(dto.Username, cancellationToken);
        if (user is null)
            throw new NotFoundException("User not found.");

        var isPasswordValid = await _userService.CheckPasswordAsync(user, dto.Password, cancellationToken);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid password.");

        var roles = await _userService.GetUserRolesAsync(user, cancellationToken);

        var tokenResult = await _jwtTokenService.GenerateTokenAsync(user, roles, cancellationToken);

        return new LoginResponseDto
        {
            Username = dto.Username,
            TokenInfo = tokenResult
        };
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.Username,
            Email = dto.Email
        };
        var roleName = dto.Role.ToString();

        await _userService.CreateUserAsync(user, dto.Password, cancellationToken);

        if (!await _userService.RoleExistsAsync(roleName, cancellationToken))
            await _userService.CreateRoleAsync(roleName, cancellationToken);

        await _userService.AddUserToRoleAsync(user, roleName, cancellationToken);

        return new RegisterResponseDto
        {
            UserId = user.Id,
            Username = user.UserName!,
            Role = roleName
        };
    }
}
