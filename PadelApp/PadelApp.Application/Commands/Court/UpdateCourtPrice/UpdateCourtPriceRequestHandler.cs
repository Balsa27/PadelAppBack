using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Court.UpdateCourtPrice;

public class UpdateCourtPriceRequestHandler : IRequestHandler<UpdateCourtPriceCommand, UpdateCourtPriceResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourtPriceRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCourtPriceResponse> Handle(UpdateCourtPriceCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);
        
        if(court is null)
            throw new CourtNotFoundException("Court not found");
        
        court.UpdateCourtPricing(request.Price);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateCourtPriceResponse(HandlerStrings.PriceUpdated);
    }
}