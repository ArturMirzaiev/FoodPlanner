using FoodPlanner.Application.Ingredients.Dtos;
using FoodPlanner.Application.Ingredients.Features.Create;
using FoodPlanner.Application.Ingredients.Features.Delete;
using FoodPlanner.Application.Ingredients.Features.GetAll;
using FoodPlanner.Application.Ingredients.Features.GetById;
using FoodPlanner.Application.Ingredients.Features.Update;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Core.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class IngredientController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<IngredientDto>?>>> GetIngredients(CancellationToken cancellationToken)
    {
        var ingredients = await mediator.Send(new GetIngredientsQuery(), cancellationToken);

        return Ok(ApiResponse<List<IngredientDto>?>.SuccessResponse(ingredients));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<IngredientDto>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var ingredient = await mediator.Send(new GetIngredientByIdQuery(id), cancellationToken);

        return Ok(
            ApiResponse<IngredientDto>.SuccessResponse(
                ingredient, IngredientMessages.RetrievedSuccessfully));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<IngredientDto>>> CreateIngredient(
        [FromBody] CreateIngredientCommand command, CancellationToken cancellationToken)
    {
        var createdIngredient = await mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = createdIngredient.Id }, 
            ApiResponse<IngredientDto>.SuccessResponse(createdIngredient, IngredientMessages.CreatedSuccessfully));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<IngredientDto>>> UpdateIngredient(
        [FromRoute] Guid id, 
        [FromBody] UpdateIngredientCommand command, 
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<IngredientDto>.FailureResponse(
                    IngredientMessages.IdMismatch, 
                IngredientMessages.InvalidRequest));
        }
        
        var updatedIngredient = await mediator.Send(command, cancellationToken);

        return Ok(
            ApiResponse<IngredientDto>.SuccessResponse(
                updatedIngredient, 
                IngredientMessages.UpdatedSuccessfully));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteIngredientCommand(id), cancellationToken);

        return NoContent();
    }
}
