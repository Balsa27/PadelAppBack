using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Commands.Court.RemoveCourt;

public record RemoveCourtCommand(Guid CourtId) : IRequest<RemoveCourtResponse>
{
    public Guid UserId { get; set; }
} 
