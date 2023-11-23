using DrealStudio.Application.Services.Interface;
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
    private readonly IUserContextService _userContextService;

    public AddCourtPriceRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<AddCourtPriceResponse> Handle(AddCourtPriceCommand request, CancellationToken cancellationToken)
    {
        var organizationId = _userContextService.GetCurrentUserId();
        
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);
        
        if (court is null)
            throw new CourtNotFoundException("Court not found");
        
        if(court.OrganizationId != organizationId)
            throw new UnauthorizedAccessException("You do not have access to this court.");

        var price = new Price(request.Amount, request.Duration, request.StartTime, request.EndTime, request.DaysOfWeek);
        
        court.AddCourtPricing(price);

        _unitOfWork.LogEntityStates();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AddCourtPriceResponse(HandlerStrings.CourtPriceAdded);
    }
}