using System.Net;

namespace FoodPlanner.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }

    protected DomainException(string message) : base(message) { }
}