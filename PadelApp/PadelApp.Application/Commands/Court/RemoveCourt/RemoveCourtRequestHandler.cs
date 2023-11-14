using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Commands.Court.RemoveCourt;

public class RemoveCourtRequestHandler : IRequestHandler<RemoveCourtCommand, RemoveCourtResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourtRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoveCourtResponse> Handle(RemoveCourtCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);

        if (court is null)
            throw new CourtNotFoundException("Court not found");

        _courtRepository.RemoveCourtAsync(court);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RemoveCourtResponse(true);
    }
}