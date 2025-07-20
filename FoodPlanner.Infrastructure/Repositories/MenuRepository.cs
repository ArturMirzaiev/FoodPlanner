using Microsoft.EntityFrameworkCore;
using FoodPlanner.Domain.Entities;
using FoodPlanner.Domain.Interfaces;
using FoodPlanner.Infrastructure.Persistence;

namespace FoodPlanner.Infrastructure.Repositories;

public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MenuRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Menu> GetActiveMenuQuery(Guid personId)
    {
        return _dbContext.Persons
            .Where(p => p.Id == personId)
            .Select(p => p.AssignedMenu)!;
    }
}
