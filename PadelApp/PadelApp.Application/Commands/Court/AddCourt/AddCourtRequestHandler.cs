using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Commands.Court.AddCourt;

public class AddCourtRequestHandler : IRequestHandler<AddCourtCommand, AddCourtResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourtRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddCourtResponse> Handle(AddCourtCommand request, CancellationToken cancellationToken)
    {
        var court = new Domain.Aggregates.Court(
            request.Name,
            request.Description,
            request.Address,
            request.WorkStartTime,
            request.WorkEndTime,
            request.Price);
        
        var existsByName = await _courtRepository.CourtExistsByNameAsync(court.Name);
        
        if (existsByName)
            throw new CourtAlreadyExistsException("Court already exists");
        
        await _courtRepository.AddCourtAsync(court);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AddCourtResponse(
            court.Id,
            court.Name,
            court.Description,
            court.Address,
            court.WorkingStartTime,
            court.WorkingEndingTime,
            court.Prices);
    }
}