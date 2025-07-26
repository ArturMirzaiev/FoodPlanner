using System.Security.Claims;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Responses;

namespace FoodPlanner.Application.Interfaces;

public interface IJwtTokenService
{
    Task<LoginResponseDto> GenerateToken(List<Claim> claims);
}