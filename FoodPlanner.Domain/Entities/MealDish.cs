namespace FoodPlanner.Domain.Entities;

public class MealDish
{
    public Guid MealId { get; set; }
    public Meal Meal { get; set; } = default!;

    public Guid DishId { get; set; }
    public Dish Dish { get; set; } = default!;
}