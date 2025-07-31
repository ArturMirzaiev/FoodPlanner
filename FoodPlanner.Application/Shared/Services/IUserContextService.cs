namespace FoodPlanner.Application.Shared.Services;

public interface IUserContextService
{
    Guid GetUserIdOrThrow();
    Guid GetUserId();
}