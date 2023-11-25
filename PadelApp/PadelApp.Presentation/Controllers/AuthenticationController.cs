using MediatR;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PadelApp.Application.Commands.Auth;
using PadelApp.Application.Commands.Organization.OrganizationRegister;
using PadelApp.Application.Commands.Player.AppleSignIn;
using PadelApp.Application.Commands.Player.GoogleSignIn;
using PadelApp.Application.Commands.Player.Login;
using PadelApp.Application.Handlers;
using PadelApp.Presentation.Contracts.Player;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;   

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("player/register")]
    public async Task<IActionResult> Register([FromBody] RegisterPlayerRequest request, CancellationToken cancellationToken)
    {
        var command = new PlayerRegisterCommand(
            request.Username,
            request.Password,
            request.Email);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginPlayerRequest request, CancellationToken cancellationToken)
    {
        var command = new UserLoginCommand(
            request.login,
            request.password);
        
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
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
    
    [HttpPost("logout/{userId}")]
    public async Task<IActionResult> Logout(string userId, CancellationToken cancellationToken)
    {
        var command = new LogoutCommand(Guid.Parse(userId));
                
        var result = await _mediator.Send(command, cancellationToken);

        return Ok();
    }
    
    [HttpPost("organization/register")]
    public async Task<IActionResult> Register([FromBody] OrganizationRegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new OrganizationRegisterCommand(
            request.Username,
            request.Email,
            request.Password,
            request.Name,
            request.Description, 
            request.Address,
            request.ContactInfo,
            request.Start,
            request.End);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("test")]
    public string Test(CancellationToken token)
    {
        return "hello!";
    }

}