using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodPlanner.API.Controllers;

[ApiController]
[Route("api/persons")]
public class PersonController : ControllerBase
{
    private readonly IMenuService _menuService;

    public PersonController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("{personId}/menu/active")]
    public async Task<ActionResult<MenuDto>> GetActiveMenu(Guid personId, CancellationToken cancellationToken)
    {
        var menuDto = await _menuService.GetActiveMenuAsync(personId, cancellationToken);
        if (menuDto == null)
            return NotFound();

        return Ok(menuDto);
    }
}