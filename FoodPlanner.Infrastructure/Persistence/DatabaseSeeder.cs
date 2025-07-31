using FoodPlanner.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodPlanner.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public DatabaseSeeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        // empty
    }
}
