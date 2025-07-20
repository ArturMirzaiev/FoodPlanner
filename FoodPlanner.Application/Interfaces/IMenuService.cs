using FoodPlanner.Application.Dtos;

namespace FoodPlanner.Application.Interfaces;

public interface IMenuService
{
    Task<MenuDto?> GetActiveMenuAsync(Guid personId, CancellationToken cancellationToken = default);
}
