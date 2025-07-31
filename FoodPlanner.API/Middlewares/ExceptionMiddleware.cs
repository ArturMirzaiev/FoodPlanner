using System.Net;
using System.Text.Json;
using FoodPlanner.Domain.Core.Common;
using FoodPlanner.Domain.Exceptions;

namespace FoodPlanner.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException dex)
        {
            await HandleDomainExceptionAsync(context, dex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exception.StatusCode;

        var response = ApiResponse<string>.FailureResponse(
            exception.SubCode,
            exception.Message
        );
        
        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
    
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentNullException => (int)HttpStatusCode.BadRequest,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var isInternalServerError = statusCode == (int)HttpStatusCode.InternalServerError;

        var response = ApiResponse<string>.FailureResponse(
            message: isInternalServerError 
                ? "Internal Server Error. Please contact support." 
                : exception.Message
        );

        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}