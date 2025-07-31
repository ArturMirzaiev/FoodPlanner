namespace FoodPlanner.Application.Authentication.Dtos;

public class LoginResponseDto
{
    public string Username { get; set; }
    public JwtTokenDto TokenInfo { get; set; }
}