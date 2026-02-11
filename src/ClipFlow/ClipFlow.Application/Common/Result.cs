namespace ClipFlow.Application.Common;

public sealed class Result
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public ErrorType ErrorType { get; set; }
    public string? ErrorMessage { get; set; }

    private Result(bool isSuccess, ErrorType errorType, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }
    
    public static Result Success() =>
        new Result(true, ErrorType.None, null);
    
    public static Result Failure(ErrorType errorType, string errorMessage) =>
        new Result(false, errorType, errorMessage);
}

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public T? Value { get; }
    public ErrorType ErrorType { get; }
    public string? ErrorMessage { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        ErrorType = ErrorType.None;
    }

    private Result(ErrorType type, string message)
    {
        IsSuccess = false;
        ErrorType = type;
        ErrorMessage = message;
    }

    public static Result<T> Success(T value) =>
        new(value);

    public static Result<T> Failure(ErrorType type, string message) =>
        new(type, message);
}