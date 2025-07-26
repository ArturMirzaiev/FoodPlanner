using System.Net;

namespace FoodPlanner.Domain.Exceptions;

public class ForbiddenException : DomainException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    public ForbiddenException(string message) : base(message) { }
}