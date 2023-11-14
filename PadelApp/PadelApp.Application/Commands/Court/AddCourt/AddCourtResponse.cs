using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourt;

public record AddCourtResponse(
    Guid CourtId,
    string Name,
    string Description,
    Address Address,
    DateTime WorkStartTime,
    DateTime WorkEndTime, 
    List<Price> Prices);
