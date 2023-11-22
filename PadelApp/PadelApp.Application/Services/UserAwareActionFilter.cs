using DrealStudio.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PadelApp.Application.Services;

public class UserAwareActionFilter : IActionFilter
{
    private readonly IUserContextService _userContextService;

    public UserAwareActionFilter(IUserContextService userContextService)
    {
        _userContextService = userContextService;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg is IUserAwareRequest userAwareRequest)
            {
                userAwareRequest.UserId = _userContextService.GetCurrentUserId();
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}