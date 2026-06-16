namespace MobiGate.Domain.Common;

public interface IResult
{
    bool IsSuccess { get; }
    int? StatusCode { get; }
    string? Error { get; }
}

public class Result<T> : IResult
{
    public bool IsSuccess { get; private init; }
    public T? Value { get; private init; }
    public int? StatusCode { get; private init; }
    public string? Error { get; private init; }

    public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<T> Failure(string error, int statusCode) => new()
    {
        IsSuccess = false,
        Error = error,
        StatusCode = statusCode
    };
}
