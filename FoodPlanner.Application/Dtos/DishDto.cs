namespace FoodPlanner.Application.Dtos;

public class DishDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsPurchased { get; set; }
    public List<DishIngredientDto> Ingredients { get; set; }
}
