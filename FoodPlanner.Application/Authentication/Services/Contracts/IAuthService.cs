using FoodPlanner.Application.Authentication.Dtos;

namespace FoodPlanner.Application.Authentication.Services.Contracts;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken);
    Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
}