using FoodPlanner.Domain.Enums;

namespace FoodPlanner.Application.Authentication.Dtos;

public class RegisterDto
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserRole Role { get; set; } = UserRole.User; 
}
