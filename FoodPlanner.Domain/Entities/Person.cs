namespace FoodPlanner.Domain.Entities;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;

    public Guid? AssignedMenuId { get; set; }
    public Menu? AssignedMenu { get; set; }
}
