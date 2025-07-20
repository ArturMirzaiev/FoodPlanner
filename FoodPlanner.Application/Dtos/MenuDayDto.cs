namespace FoodPlanner.Application.Dtos;

public class MenuDayDto
{
    public int DayNumber { get; set; } // Наприклад, "1", "2", "3"
    public List<MealDto> Meals { get; set; } = new();
}
