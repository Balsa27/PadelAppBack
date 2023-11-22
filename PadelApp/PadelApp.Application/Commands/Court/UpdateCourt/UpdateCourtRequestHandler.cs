using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Court.UpdateCourt;

public class UpdateCourtRequestHandler : IRequestHandler<UpdateCourtCommand, UpdateCourtResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public UpdateCourtRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<UpdateCourtResponse> Handle(UpdateCourtCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);
        var id = _userContextService.GetCurrentUserId();
        
        if (court is null)
            throw new CourtNotFoundException("Court not found");
        
        court.UpdateCourtDetails(request.Name, request.Description, request.Address, request.WorkStartTime, request.WorkEndTime);
        
        _courtRepository.Update(court);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateCourtResponse(HandlerStrings.CourtUpdated);
    }
}