using FoodPlanner.Application.Dtos;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Responses;

namespace FoodPlanner.Application.Interfaces;

public interface IAuthService
{
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken);
    Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
}