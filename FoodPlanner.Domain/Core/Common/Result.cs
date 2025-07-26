namespace FoodPlanner.Domain.Core.Common;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public List<string> Errors { get; protected set; } = new();

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        if (errors != null)
            Errors = errors;
    }

    public static Result Success() => new(true);

    public static Result Failure(params string[] errors) =>
        new(false, errors.ToList());

    public static Result Failure(List<string> errors) =>
        new(false, errors);
}

public class Result<T> : Result
{
    public T? Value { get; private set; }

    private Result(T value) : base(true)
    {
        Value = value;
    }

    private Result(List<string> errors) : base(false, errors)
    {
        Value = default;
    }

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(params string[] errors) =>
        new(errors.ToList());

    public static new Result<T> Failure(List<string> errors) =>
        new(errors);
}
