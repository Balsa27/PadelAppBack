using MediatR;

namespace PadelApp.Application.Handlers;

public record PlayerRegisterCommand(string Username, string Password, string Email) 
    : IRequest<PlayerRegisterResponse>;