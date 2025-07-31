using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Authentication.Services.Contracts;

public interface IJwtTokenService
{
    Task<JwtTokenDto> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles, CancellationToken cancellationToken);
}