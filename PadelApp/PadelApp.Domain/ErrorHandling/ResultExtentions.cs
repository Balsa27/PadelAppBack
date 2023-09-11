using MediatR;

public static class ResultExtensions
{
    public static async Task<Result<TOutput>> OnSuccessAsync<TInput, TOutput>(
        this Task<Result<TInput>> resultTask,
        Func<TInput, Task<Result<TOutput>>> func)
    {
        var result = await resultTask;
        if (!result.IsSuccess)
            return Result<TOutput>.Fail(result.Error);

        return await func(result.Value);
    }
    
    public static async Task<Result<Unit>> OnSuccessAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<Unit>>> func)
    {
        var result = await resultTask;
        if (!result.IsSuccess)
            return Result<Unit>.Fail(result.Error);

        return await func(result.Value);
    }
}