// using DrealStudio.Application.Services.Interface;
// using MediatR.Pipeline;
//
// namespace PadelApp.Application.Services;
//
// public class UserContextPreProcessor<TRequest> 
//     : IRequestPreProcessor<TRequest> where TRequest : notnull
// {
//     private readonly IUserContextService _userContextService;
//
//     public UserContextPreProcessor(IUserContextService userContextService)
//     {
//         _userContextService = userContextService;
//     }
//
//     public Task Process(TRequest request, CancellationToken cancellationToken)
//     {
//         if (request is IUserAwareRequest userAwareRequest)
//             userAwareRequest.UserId = _userContextService.GetCurrentUserId();
//        
//         return Task.CompletedTask;
//     }
// }