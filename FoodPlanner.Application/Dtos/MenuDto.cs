namespace FoodPlanner.Application.Dtos;

public class MenuDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<MenuDayDto> Days { get; set; } = new();
}
