using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Authentication.Services.Contracts;

public interface IUserService
{
    Task<ApplicationUser?> FindByNameAsync(string username, CancellationToken cancellationToken);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
    Task CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
    Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken);
    Task AddUserToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken);
    Task CreateRoleAsync(string roleName, CancellationToken cancellationToken);
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default);
}