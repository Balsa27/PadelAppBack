using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.Commands.Player.DeleteUser;
using PadelApp.Domain.Enums;
using PadelApp.Presentation.Attributes;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Token(Role.Player)]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var command = new DeletePlayerCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}