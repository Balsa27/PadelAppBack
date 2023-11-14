using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Court.CourtById;

public record CourtByIdCommand(Guid CourtId) : IRequest<CourtByIdResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
