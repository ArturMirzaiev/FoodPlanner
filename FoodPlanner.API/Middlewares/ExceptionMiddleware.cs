using System.Net;
using System.Text.Json;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Exceptions;
using FoodPlanner.Domain.Responses;

namespace FoodPlanner.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
    
        switch (exception)
        {
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                break;
            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                break;
            case DomainException domainEx:
                code = domainEx.StatusCode;
                break;
        }

        var errorDetails = new ErrorDetails
        {
            StatusCode = (int)code,
            Message = exception.Message,
            Details = exception.InnerException?.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorDetails.StatusCode;

        return context.Response.WriteAsync(errorDetails.ToString());
    }
}