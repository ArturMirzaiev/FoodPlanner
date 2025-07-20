namespace FoodPlanner.Domain.Entities;

public class Meal
{
    public Guid Id { get; set; }

    public string Type { get; set; } = default!;
    public TimeOnly? Time { get; set; }

    public Guid MenuDayId { get; set; }
    public MenuDay MenuDay { get; set; } = default!;

    public ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
}
