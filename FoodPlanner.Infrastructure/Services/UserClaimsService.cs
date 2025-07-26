using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodPlanner.Infrastructure.Services;

public class UserClaimsService : IUserClaimsService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserClaimsService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}