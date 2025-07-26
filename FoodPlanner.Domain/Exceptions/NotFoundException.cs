using System.Net;

namespace FoodPlanner.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public NotFoundException(string message) : base(message) { }
}