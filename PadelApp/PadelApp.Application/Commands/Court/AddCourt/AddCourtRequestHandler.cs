using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourt;

public class AddCourtRequestHandler : IRequestHandler<AddCourtCommand, AddCourtResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public AddCourtRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<AddCourtResponse> Handle(AddCourtCommand request, CancellationToken cancellationToken)
    {
        var id = _userContextService.GetCurrentUserId();
        
        var existsByName = await _courtRepository.CourtExistsByNameAsync(request.Name);
        
        if (existsByName)
            throw new CourtAlreadyExistsException("Court already exists");
        
        var prices = request.Prices
            .Select(p => new Price(
                p.Amount,
                p.Duration, 
                p.TimeStart,
                p.TimeEnd, 
                p.Days))
            .ToList();  

        var court = new Domain.Aggregates.Court(
            id,
            request.Name,
            request.Description,
            request.Address,
            request.WorkStartTime,
            request.WorkEndTime,
            prices,
            request.imageUrl,
            request.courtImages);
        
        await _courtRepository.AddCourtAsync(court);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AddCourtResponse(
            court.Id,
            id,
            request.Name,
            request.Description,
            court.Address,
            request.WorkStartTime,
            request.WorkEndTime,
            prices,
            request.imageUrl,
            request.courtImages,
            court.Status);
    }
}