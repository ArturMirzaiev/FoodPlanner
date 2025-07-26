namespace FoodPlanner.Domain.Responses;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}