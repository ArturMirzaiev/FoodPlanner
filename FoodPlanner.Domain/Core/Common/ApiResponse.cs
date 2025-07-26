namespace FoodPlanner.Domain.Core.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public List<string>? Errors { get; init; }
    public T? Data { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null) => new()
    {
        Success = true,
        Message = message,
        Data = data
    };

    public static ApiResponse<T> FailureResponse(List<string> errors, string? message = null) => new()
    {
        Success = false,
        Message = message,
        Errors = errors
    };
}
