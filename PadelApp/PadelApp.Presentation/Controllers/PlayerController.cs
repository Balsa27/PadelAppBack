using MediatR;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PadelApp.Application.Commands.Player.AppleSignIn;
using PadelApp.Application.Commands.Player.GoogleSignIn;
using PadelApp.Application.Commands.Player.Login;
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
        
        return Created(string.Empty, new { Token = result.Value });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginPlayerRequest request, CancellationToken cancellationToken)
    {
        var command = new PlayerLoginCommand(
            request.login,
            request.password);
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { Error = result.Error.Message });
        
        return Ok(new { Token = result.Value });
    }

    [HttpPost("google-signin")]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GoogleSignInCommand(request.GoogleToken);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(new { Error = result.Error.Message });
        
        return Created(string.Empty, new { Token = result.Value.JwtToken});
    }

    [HttpPost("apple-signin")]
    public async Task<IActionResult> AppleSignIn([FromBody] AppleSignInRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AppleSignInCommand(request.AppleToken);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(new { Error = result.Error.Message });
        
        return Ok(new { string.Empty, Token = result.Value.Token});
    }

    [HttpGet("test")]
    public string Test(CancellationToken token)
    {
        return "hello!";
    }

}