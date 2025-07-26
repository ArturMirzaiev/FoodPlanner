namespace FoodPlanner.Domain.Entities;

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
