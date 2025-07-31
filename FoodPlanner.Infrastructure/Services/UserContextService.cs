using FoodPlanner.Application.Shared.Services;
using Microsoft.AspNetCore.Http;

namespace FoodPlanner.Infrastructure.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub");
        return Guid.TryParse(claim?.Value, out var userId) ? userId : Guid.Empty;
    }

    public Guid GetUserIdOrThrow()
    {
        var userId = GetUserId();
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User id not found in context.");
        return userId;
    }
}