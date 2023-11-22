using MediatR;

namespace PadelApp.Application.Commands.Auth;

public record LogoutCommand(Guid UserId) : IRequest<string>;
    
