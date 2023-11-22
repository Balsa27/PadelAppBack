using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Organization.UpdateOrganization;

public record UpdateOrganizationRequest(
    string Name,
    string Description,
    Address Address,
    ContactInfo ContactInfo,
    TimeSpan WorkingStartHours,
    TimeSpan WorkingEndingHours);
