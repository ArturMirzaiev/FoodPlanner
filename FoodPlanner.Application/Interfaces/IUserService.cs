using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Interfaces;

public interface IUserService
{
    Task<ApplicationUser?> FindByNameAsync(string username, CancellationToken cancellationToken);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
    Task<Result> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
    Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken);
    Task<Result> AddUserToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken);
    Task<Result> CreateRoleAsync(string roleName, CancellationToken cancellationToken);
}
