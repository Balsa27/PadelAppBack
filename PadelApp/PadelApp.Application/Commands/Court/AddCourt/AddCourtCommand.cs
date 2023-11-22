using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourt;

public record AddCourtCommand(
    string Name,
    string Description,
    Address Address,
    TimeSpan WorkStartTime,
    TimeSpan WorkEndTime,
    List<Price> Prices,
    string? imageUrl,
    List<string>? courtImages) : IRequest<AddCourtResponse>;