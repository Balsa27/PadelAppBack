using MediatR;
using PadelApp.Domain.Enums;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.UpdateCourt;

public record UpdateCourtCommand(Guid CourtId,
    string Name,
    string Description,
    Address Address,
    TimeSpan WorkStartTime,
    TimeSpan WorkEndTime) : IRequest<UpdateCourtResponse>;
