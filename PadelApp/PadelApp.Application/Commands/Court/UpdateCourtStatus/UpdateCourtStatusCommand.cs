using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Domain.Enums;

namespace PadelApp.Application.Commands.Court.UpdateCourtStatus;

public record UpdateCourtStatusCommand(Guid CourtId, CourtStatus Status) 
    : IRequest<UpdateCourtStatusResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
