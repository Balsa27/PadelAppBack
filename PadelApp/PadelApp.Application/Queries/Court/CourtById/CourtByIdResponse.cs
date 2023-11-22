using PadelApp.Domain.Enums;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Queries.Court.CourtById;

public record CourtByIdResponse( Guid CourtId,
    Guid OrganizationId,
    string Name,
    string Description,
    Address Address,
    TimeSpan WorkStartTime,
    TimeSpan WorkEndTime,
    List<Price> Prices,
    List<Domain.Entities.Booking> Bookings,
    string? imageUrl,
    List<string>? courtImages,
    CourtStatus Status);