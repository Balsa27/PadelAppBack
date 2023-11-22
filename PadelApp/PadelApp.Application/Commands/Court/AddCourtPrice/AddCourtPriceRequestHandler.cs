using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Commands.Court.UpdateCourtPrice;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourtPrice;

public class AddCourtPriceRequestHandler : IRequestHandler<AddCourtPriceCommand, AddCourtPriceResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourtPriceRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddCourtPriceResponse> Handle(AddCourtPriceCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);
        
        if (court is null)
            throw new CourtNotFoundException("Court not found");

        var price = new Price(request.Amount, request.Duration, request.StartTime, request.EndTime, request.DaysOfWeek);
        
        court.AddCourtPricing(price);

        _unitOfWork.LogEntityStates();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AddCourtPriceResponse(HandlerStrings.CourtPriceAdded);
    }
}