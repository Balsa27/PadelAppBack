using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourtPrice;

public record AddCourtPriceRequest(Guid CourtId, decimal amount, TimeSpan duration, TimeSpan startTime, TimeSpan endTime, List<DayOfWeek> daysOfWeek);
