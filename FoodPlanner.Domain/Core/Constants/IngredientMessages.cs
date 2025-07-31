namespace FoodPlanner.Domain.Core.Constants;

public static class IngredientMessages
{
    public const string NotFound = "Ingredient not found.";
    public const string RetrievedSuccessfully = "Ingredient retrieved successfully.";
    public const string CreatedSuccessfully = "Ingredient created successfully.";
    public const string UpdatedSuccessfully = "Ingredient updated successfully.";
    public const string DeletedSuccessfully = "Ingredient deleted successfully.";
    public const string IdMismatch = "Id in URL and command body do not match.";
    public const string InvalidRequest = "Invalid request.";
    public const string AccessDenied = "Access denied.";
}