public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error error = null)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException();
        if (!isSuccess && error == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true);

    public static Result Fail(Error error) => new Result(false, error);

    public Result OnSuccess(Func<Result> next)
    {
        if (IsSuccess)
            return next.Invoke();
        return this;
    }

    public Result OnFailure(Func<Result> next)
    {
        if (IsFailure)
            return next.Invoke();
        return this;
    }
    
    
    public async Task<Result> OnSuccessAsync(Func<Task<Result>> next)
    {
        if (IsSuccess)
            return await next.Invoke();
        return this;
    }

    public async Task<Result> OnFailureAsync(Func<Task<Result>> next)
    {
        if (IsFailure)
            return await next.Invoke();
        return this;
    }
}