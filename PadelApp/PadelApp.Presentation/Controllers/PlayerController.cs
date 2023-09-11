using MediatR;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PadelApp.Application.Handlers;
using PadelApp.Presentation.Contracts.Player;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("/api/auth")]
public class PlayerController : ControllerBase
{
    private readonly IMediator _mediator;   

    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterPlayerRequest request, CancellationToken cancellationToken)
    {
        var command = new PlayerRegisterCommand(
            request.Username,
            request.Password,
            request.Email);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { Error = result.Error.Message });
        
        return Ok(new { result.Value });
    }
}