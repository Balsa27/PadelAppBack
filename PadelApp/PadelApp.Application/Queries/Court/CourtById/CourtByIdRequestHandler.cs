using MediatR;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Queries.Court.CourtById;

public class CourtByIdRequestHandler : IRequestHandler<CourtByIdCommand, CourtByIdResponse>
{
    private readonly ICourtRepository _courtRepository;

    public CourtByIdRequestHandler(ICourtRepository courtRepository)
    {
        _courtRepository = courtRepository;
    }

    public async Task<CourtByIdResponse> Handle(CourtByIdCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);
        
        if (court is null)
            throw new CourtNotFoundException($"Court with id {request.CourtId} not found");

        return new CourtByIdResponse(
            court.Id,
            court.Name,
            court.Description,
            court.Address,
            court.Prices,
            court.ProfileImage,
            court.CourtImages
            );
    }
}