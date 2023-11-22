using PadelApp.Domain.Enums;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourt;

public record AddCourtResponse(
    Guid CourtId,
    Guid OrganizationId,
    string Name,
    string Description,
    Address Address,
    TimeSpan WorkStartTime,
    TimeSpan WorkEndTime,
    List<Price> Prices,
    string? imageUrl,
    List<string>? courtImages,
    CourtStatus Status);
