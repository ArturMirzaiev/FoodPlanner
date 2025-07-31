using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodPlanner.Application.Authentication.Dtos;
using FoodPlanner.Application.Authentication.Services.Contracts;
using FoodPlanner.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FoodPlanner.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;
    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public Task<JwtTokenDto> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles, CancellationToken cancellationToken)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var expires = DateTime.UtcNow.AddHours(2);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: expires,
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(new JwtTokenDto
        {
            Token = tokenString,
            Expires = expires,
        });
    }
}