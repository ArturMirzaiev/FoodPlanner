using System.Net;
using FoodPlanner.Domain.Core.Constants;

namespace FoodPlanner.Domain.Exceptions;

public class IngredientException : DomainException
{
    private IngredientException(string message, HttpStatusCode statusCode, string subCode)
        : base(message, statusCode, subCode)
    {
    }

    public static IngredientException NotFound(Guid id)
    {
        return new IngredientException(
            $"Ingredient with id '{id}' not found.",
            HttpStatusCode.NotFound,
            SubCodes.Ingredient.NotFound
        );
    }

    public static IngredientException NotOwned(Guid id)
    {
        return new IngredientException(
            $"Ingredient with id '{id}' is not owned by the user.",
            HttpStatusCode.Forbidden,
            SubCodes.Ingredient.NotOwned
        );
    }
    
    public static IngredientException IsReadOnly(Guid id)
    {
        return new IngredientException(
            $"Ingredient with id '{id}' is system-defined and cannot be modified.",
            HttpStatusCode.BadRequest,
            SubCodes.Ingredient.IsReadOnly
        );
    }
    
    public static IngredientException AccessDenied(Guid userId) =>
        new IngredientException($"User '{userId}' does not have access to the ingredient.",
            HttpStatusCode.Forbidden,
            SubCodes.Ingredient.AccessDenied);
}
