using System.Net;

namespace FoodPlanner.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string SubCode { get; }

    public DomainException(string message, HttpStatusCode statusCode, string subCode)
        : base(message)
    {
        StatusCode = statusCode;
        SubCode = subCode;
    }
}
