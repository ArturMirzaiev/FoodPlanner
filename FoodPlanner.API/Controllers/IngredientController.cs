using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FoodPlanner.Application.Dtos;
using FoodPlanner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class IngredientController : ControllerBase
{
    private readonly IIngredientService _ingredientService;

    public IngredientController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableIngredients(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var ingredients = await _ingredientService.GetAvailableIngredientsAsync(userId, cancellationToken);
        return Ok(ingredients);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var ingredient = await _ingredientService.GetByIdAsync(id, userId, cancellationToken);

        if (ingredient is null)
            return NotFound();

        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIngredientDto createIngredientDto, CancellationToken cancellationToken)
    {
        if (createIngredientDto == null)
            return BadRequest("Ingredient data is required.");

        var userId = GetCurrentUserId();
        var createdIngredient = await _ingredientService.AddAsync(createIngredientDto, userId, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = createdIngredient.Id }, createdIngredient);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIngredientDto updateIngredientDto, CancellationToken cancellationToken)
    {
        if (updateIngredientDto == null)
            return BadRequest("Ingredient data is required.");

        if (id != updateIngredientDto.Id)
            return BadRequest("Id mismatch.");

        var userId = GetCurrentUserId();

        try
        {
            await _ingredientService.UpdateAsync(updateIngredientDto, userId, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _ingredientService.DeleteAsync(id, userId, cancellationToken);
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("User is not authenticated or userId claim is invalid.");

        return userId;
    }
}
