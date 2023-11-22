using MediatR;

namespace PadelApp.Application.Commands.Organization.RemoveOrganization;

public record RemoveOrganizationCommand() : IRequest<RemoveOrganizationResponse>;
