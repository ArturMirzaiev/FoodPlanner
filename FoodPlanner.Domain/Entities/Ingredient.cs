namespace FoodPlanner.Domain.Entities;

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Unit { get; set; } = "g"; // "г", "мл", "шт"
}
