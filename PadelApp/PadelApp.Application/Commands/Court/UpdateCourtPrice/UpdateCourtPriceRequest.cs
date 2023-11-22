using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.UpdateCourtPrice;

public record UpdateCourtPriceRequest(Guid CourtId, Price Price);

