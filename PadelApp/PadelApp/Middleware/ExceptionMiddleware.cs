using Newtonsoft.Json;
using PadelApp.Application.Exceptions;

namespace PadelApp.Middleware;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        switch (exception)
        {
            case UnauthorizedAccessException _:
                statusCode = StatusCodes.Status401Unauthorized;
                break;
            case AccessViolationException _:
                statusCode = StatusCodes.Status403Forbidden;
                break;
            case OrganizationAlreadyExistsException _:
                statusCode = StatusCodes.Status409Conflict;
                break;
            case CourtNotFoundException _:
                statusCode = StatusCodes.Status404NotFound;
                break;
            case PriceNotFoundException _:
                statusCode = StatusCodes.Status404NotFound;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }
        context.Response.StatusCode = statusCode;

        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        return context.Response.WriteAsync(result);
    }
}