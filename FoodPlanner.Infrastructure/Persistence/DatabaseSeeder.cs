using FoodPlanner.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        
        if (await _context.Menus.AnyAsync())
            return;

        var ingredient1 = new Ingredient { Id = Guid.NewGuid(), Name = "Помидор", Unit = "шт" };
        var ingredient2 = new Ingredient { Id = Guid.NewGuid(), Name = "Огурец", Unit = "шт" };
        var ingredient3 = new Ingredient { Id = Guid.NewGuid(), Name = "Перец сладкий", Unit = "шт" };

        var dish1 = new Dish
        {
            Id = Guid.NewGuid(),
            Name = "Салат Овощной",
            IsPurchased = false,
            Ingredients = new List<DishIngredient>
            {
                new DishIngredient { Ingredient = ingredient1, Quantity = 2 },
                new DishIngredient { Ingredient = ingredient2, Quantity = 1 },
                new DishIngredient { Ingredient = ingredient3, Quantity = 1 }
            }
        };

        var mealDish = new MealDish
        {
            DishId = dish1.Id,
            Dish = dish1
        };
        
        var meal = new Meal
        {
            Id = Guid.NewGuid(),
            Type = "Завтрак",
            Time = new TimeOnly(8, 0),
            MealDishes = new List<MealDish> { mealDish }
        };

        var day = new MenuDay
        {
            Order = 1,
            Meals = new List<Meal> { meal }, 
            Id = Guid.NewGuid()
        };

        var menu = new Menu
        {
            Id = Guid.NewGuid(),
            Title = "Меню на неделю",
            StartFrom = DateOnly.FromDateTime(DateTime.Today),
            Days = new List<MenuDay> { day }
        };
        
        var user = new ApplicationUser
        {
            UserName = "testuser",
            Email = "test@example.com",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(user, "StrongP@ssw0rd!");

        var person = new Person
        {
            Name = "Иван",
            UserId = user.Id,
            AssignedMenu = menu
        };

        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
    }
}
