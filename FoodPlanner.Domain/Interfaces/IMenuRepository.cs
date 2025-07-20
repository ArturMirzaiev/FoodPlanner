using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Domain.Interfaces;

public interface IMenuRepository : IRepositoryBase<Menu>
{
    IQueryable<Menu> GetActiveMenuQuery(Guid personId);
}