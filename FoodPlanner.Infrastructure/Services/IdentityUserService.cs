using FoodPlanner.Application.Interfaces;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodPlanner.Infrastructure.Services;

public class IdentityUserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentityUserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string username, CancellationToken cancellationToken)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<Result> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded) 
            return Result.Success();

        return Result.Failure(result.Errors.Select(e => e.Description).ToList());
    }

    public async Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }
    
    public async Task<Result> CreateRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        if (result.Succeeded) 
            return Result.Success();
        
        return Result.Failure(result.Errors.Select(e => e.Description).ToList());
    }

    public async Task<Result> AddUserToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded) return Result.Success();

        return Result.Failure(result.Errors.Select(e => e.Description).ToList());
    }
}