using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Infrastructure.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> AsTrackingIf<T>(this IQueryable<T> queryable, bool trackChanges) where T : class 
        => trackChanges ? queryable.AsTracking() : queryable.AsNoTracking();
}