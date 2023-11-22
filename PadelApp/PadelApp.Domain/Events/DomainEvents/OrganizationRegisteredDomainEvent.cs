using PadelApp.Domain.Primitives;

namespace PadelApp.Application.Events.Domain;

public record OrganizationRegisteredDomainEvent(Guid OrganizationId, string Name, string Email) : IDomainEvent;
