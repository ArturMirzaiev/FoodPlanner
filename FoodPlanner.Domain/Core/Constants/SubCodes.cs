namespace FoodPlanner.Domain.Core.Constants;

public static class SubCodes
{
    public static class Ingredient
    {
        public const string NotFound = "INGREDIENT_NOT_FOUND";
        public const string NotOwned = "INGREDIENT_NOT_OWNED";
        public const string IsReadOnly = "INGREDIENT_IS_READ_ONLY";
        public const string AccessDenied = "ACCESS_DENIED";
    }
    
    public static class Auth
    {
        public const string Unauthorized = "AUTH_UNAUTHORIZED";
        public const string Forbidden = "AUTH_FORBIDDEN";

        public const string UserNotFound = "USER_NOT_FOUND";
        public const string InvalidPassword = "INVALID_PASSWORD";
        public const string NoClaims = "NO_CLAIMS";

        public const string RoleCreationFailed = "ROLE_CREATION_FAILED";
        public const string RoleAssignmentFailed = "ROLE_ASSIGNMENT_FAILED";
        public const string UserCreationFailed = "USER_CREATION_FAILED";
    }

    public static class Common
    {
        public const string UnexpectedError = "UNEXPECTED_ERROR";
    }
}