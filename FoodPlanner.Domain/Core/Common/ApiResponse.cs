namespace FoodPlanner.Domain.Core.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string? Code { get; init; }
    public T? Data { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null) => new()
    {
        Success = true,
        Message = message,
        Data = data
    };

    public static ApiResponse<T> FailureResponse(string? code = null, string? message = null) => new()
    {
        Success = false,
        Message = message
    };
}
