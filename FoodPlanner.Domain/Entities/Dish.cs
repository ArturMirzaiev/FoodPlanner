namespace FoodPlanner.Domain.Entities;

public class Dish
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsPurchased { get; set; }

    public ICollection<MealDish> MealDishes { get; set; } = new List<MealDish>();
    public ICollection<DishIngredient> Ingredients { get; set; } = new List<DishIngredient>();
}