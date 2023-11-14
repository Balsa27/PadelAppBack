using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourt;

public record AddCourtRequest(
    string Name,
    string Description,
    Address Address,
    DateTime WorkStartTime,
    DateTime WorkEndTime,
    Price Price);