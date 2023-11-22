using MediatR;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Court.GetOrganizationCourts;

public class OrganizationCourtsRequestHandler : IRequestHandler<OrganizationCourtsCommand, List<OrganizationCourtResponse>>
{
    private readonly ICourtRepository _courtRepository;

    public OrganizationCourtsRequestHandler(ICourtRepository courtRepository)
    {
        _courtRepository = courtRepository;
    }

    public async Task<List<OrganizationCourtResponse>> Handle(OrganizationCourtsCommand request, CancellationToken cancellationToken)
    {
        var courts = await _courtRepository.GetOrganizationCourts(request.OrganizationId);

        if (courts is null)
            return new List<OrganizationCourtResponse>();

        return courts.Select(c => new OrganizationCourtResponse(
            c.Id,
            c.OrganizationId,
            c.Name,
            c.Description,
            c.Address,
            c.WorkingStartTime,
            c.WorkingEndTime,
            c.Prices,
            c.Bookings,
            c.ProfileImage,
            c.CourtImages,
            c.Status)).ToList();
    }
}