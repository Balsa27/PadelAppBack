using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Court.RemoveCourt;

public class RemoveCourtRequestHandler : IRequestHandler<RemoveCourtCommand, RemoveCourtResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public RemoveCourtRequestHandler(
        ICourtRepository courtRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<RemoveCourtResponse> Handle(RemoveCourtCommand request, CancellationToken cancellationToken)
    {
        var id = _userContextService.GetCurrentUserId();
        
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);

        if (court is null)
            throw new CourtNotFoundException("Court not found");

        _courtRepository.RemoveCourtAsync(court);
        
        court.RemoveCourtFromOrganization(id, court.Id);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RemoveCourtResponse(HandlerStrings.CourtDeleted);
    }
}