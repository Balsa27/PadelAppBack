using MediatR;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.DomainEvents;

public record PlayerRegisteredDomainEvent(Guid PlayerId, string Username, string Email) : IDomainEvent;
