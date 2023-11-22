using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Player.DeleteUser;

public class DeletePlayerRequestHandler : IRequestHandler<DeletePlayerCommand, string>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeletePlayerRequestHandler(
        IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork, 
        IUserContextService userContextService)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<string> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetById(
            _userContextService.GetCurrentUserId());

        if (player is null)
            throw new UserNotFoundException("User not found");

        player.Deactivate();
        _playerRepository.Update(player);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return HandlerStrings.UserDeleted;
    }
}