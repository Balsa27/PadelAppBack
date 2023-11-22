using MediatR;

namespace PadelApp.Application.Queries.Court.GetOrganizationCourts;

public record OrganizationCourtsCommand(Guid OrganizationId) : IRequest<List<OrganizationCourtResponse>>;
