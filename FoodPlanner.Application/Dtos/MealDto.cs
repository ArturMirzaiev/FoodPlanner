namespace FoodPlanner.Application.Dtos;

public class MealDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } // Наприклад, "Сніданок"
    public TimeOnly Time { get; set; }
    public List<MealDishDto> Dishes { get; set; } = new();
}
