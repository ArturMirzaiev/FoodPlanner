using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodPlanner.Application.Interfaces;

namespace FoodPlanner.API.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var claim = user?.FindFirst(ClaimTypes.NameIdentifier) ?? user?.FindFirst(JwtRegisteredClaimNames.Sub);

        if (claim == null || !Guid.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid or missing user ID.");

        UserId = userId;
    }
}