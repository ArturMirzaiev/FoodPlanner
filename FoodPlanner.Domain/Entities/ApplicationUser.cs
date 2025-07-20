using Microsoft.AspNetCore.Identity;

namespace FoodPlanner.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public ICollection<Person> Persons { get; set; } = new List<Person>();
}