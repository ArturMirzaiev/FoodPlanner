using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Responses;
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

    public Task<LoginResponseDto> GenerateToken(List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var expires = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: expires,
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new LoginResponseDto
        {
            Token = tokenString,
            Expires = expires,
        };

        return Task.FromResult(response);
    }
}