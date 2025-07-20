namespace FoodPlanner.Domain.Entities;

public class DishIngredient
{
    public Guid DishId { get; set; }
    public Dish Dish { get; set; } = default!;

    public Guid IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = default!;

    public decimal Quantity { get; set; } // в грамах/мл/шт
}
