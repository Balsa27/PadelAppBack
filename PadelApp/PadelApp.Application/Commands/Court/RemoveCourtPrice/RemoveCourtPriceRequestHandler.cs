using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Court.RemoveCourtPrice;

public class RemoveCourtPriceRequestHandler : IRequestHandler<RemoveCourtPriceCommand, RemoveCourtPriceResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourtPriceRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoveCourtPriceResponse> Handle(RemoveCourtPriceCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);

        if (court is null)
            throw new PriceNotFoundException("Price not found");
        
        court.RemoveCourtPricing(request.PriceId);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new RemoveCourtPriceResponse(HandlerStrings.PriceRemoved);
    }
}