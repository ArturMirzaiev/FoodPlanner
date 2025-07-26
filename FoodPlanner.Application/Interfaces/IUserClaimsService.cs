using System.Security.Claims;
using FoodPlanner.Domain.Entities;

namespace FoodPlanner.Application.Interfaces;

public interface IUserClaimsService
{
    Task<List<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken);
}