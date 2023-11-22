using MediatR;

namespace PadelApp.Application.Commands.Auth;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, string>
{
    public Task<string> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}