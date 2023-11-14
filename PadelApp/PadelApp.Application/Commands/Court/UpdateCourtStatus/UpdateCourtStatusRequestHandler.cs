using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Commands.Court.UpdateCourtStatus;

public class UpdateCourtStatusRequestHandler : IRequestHandler<UpdateCourtStatusCommand, UpdateCourtStatusResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourtStatusRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCourtStatusResponse> Handle(UpdateCourtStatusCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);
        
        if (court is null)
            throw new CourtNotFoundException("Court not found");
        
        court.UpdateCourtStatus(request.Status); 

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateCourtStatusResponse(true);
    }
}