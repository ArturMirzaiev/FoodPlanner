namespace FoodPlanner.Application.Dtos;

public class DishIngredientDto
{
    public Guid IngredientId { get; set; }
    public IngredientDto Ingredient { get; set; }
    public decimal Quantity { get; set; }
}
