public class Result<T> : Result
{
    private readonly T _value;

    public T Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    private Result(T value) : base(true)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
    }

    public static Result<T> Success(T value) => new(value);

    public new static Result<T> Fail(Error error) => new(error);

    public Result<T> OnSuccess(Func<T, Result<T>> next)
    {
        if (IsSuccess)
            return next.Invoke(_value);
        return this;
    }

    public Result<T> OnFailure(Func<Result<T>> next)
    {
        if (IsFailure)
            return next.Invoke();
        return this;
    }
    
    public async Task<Result<T>> OnSuccessAsync(Func<T, Task<Result<T>>> next)
    {
        if (IsSuccess)
            return await next.Invoke(_value);
        return this;
    }

    public async Task<Result<T>> OnFailureAsync(Func<Task<Result<T>>> next)
    {
        if (IsFailure)
            return await next.Invoke();
        return this;
    }

}