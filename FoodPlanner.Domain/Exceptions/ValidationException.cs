using System.Net;

namespace FoodPlanner.Domain.Exceptions;

public class ValidationException : DomainException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public ValidationException(string message) : base(message) { }
}