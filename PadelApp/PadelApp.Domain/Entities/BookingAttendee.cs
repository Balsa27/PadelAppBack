using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Primitives;

namespace PadelApp.Persistance.Dbos;

public class BookingAttendee : Entity
{
    public Guid BookingId { get; init; }
    public Guid PlayerId { get; init; }
}