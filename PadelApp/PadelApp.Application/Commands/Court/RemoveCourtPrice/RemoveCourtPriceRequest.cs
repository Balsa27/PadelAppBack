using MediatR;

namespace PadelApp.Application.Commands.Court.RemoveCourtPrice;

public record RemoveCourtPriceRequest(Guid CourtId, Guid PriceId);
