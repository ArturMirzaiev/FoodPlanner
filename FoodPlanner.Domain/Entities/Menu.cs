namespace FoodPlanner.Domain.Entities;

public class Menu
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public DateOnly StartFrom { get; set; }

    public ICollection<MenuDay> Days { get; set; } = new List<MenuDay>();
}
