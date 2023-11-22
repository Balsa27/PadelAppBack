using MediatR;
using PadelApp.Application.Commands.Court.UpdateCourtPrice;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourtPrice;

public record AddCourtPriceCommand(Guid CourtId, decimal Amount, TimeSpan Duration, TimeSpan StartTime, TimeSpan EndTime, List<DayOfWeek> DaysOfWeek) : IRequest<AddCourtPriceResponse>;
