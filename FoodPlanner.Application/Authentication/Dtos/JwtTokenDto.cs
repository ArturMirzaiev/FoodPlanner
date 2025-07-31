namespace FoodPlanner.Application.Authentication.Dtos;

public class JwtTokenDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}