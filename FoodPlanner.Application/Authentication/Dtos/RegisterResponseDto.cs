namespace FoodPlanner.Application.Authentication.Dtos;

public class RegisterResponseDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
}
