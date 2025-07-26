using Microsoft.AspNetCore.Identity;

namespace FoodPlanner.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}