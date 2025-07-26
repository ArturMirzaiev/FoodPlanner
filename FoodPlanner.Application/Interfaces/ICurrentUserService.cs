namespace FoodPlanner.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
}