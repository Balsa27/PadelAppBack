using MediatR;

namespace PadelApp.Application.Commands.Court.RemoveCourtPrice;

public record RemoveCourtPriceCommand(Guid CourtId, Guid PriceId) : IRequest<RemoveCourtPriceResponse>;
