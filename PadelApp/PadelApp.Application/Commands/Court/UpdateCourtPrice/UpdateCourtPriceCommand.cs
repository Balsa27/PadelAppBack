using MediatR;
using PadelApp.Application.Commands.Court.UpdateCourt;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.UpdateCourtPrice;

public record UpdateCourtPriceCommand(Guid CourtId, Price Price) : IRequest<UpdateCourtPriceResponse>;
