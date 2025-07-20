namespace FoodPlanner.Domain.Entities;

public class MenuDay
{
    public Guid Id { get; set; }
    public int Order { get; set; } // 1, 2, 3…

    public Guid MenuId { get; set; }
    public Menu Menu { get; set; } = default!;

    public ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public DateOnly Date => Menu.StartFrom.AddDays(Order - 1); // можно маппить на DTO
}
