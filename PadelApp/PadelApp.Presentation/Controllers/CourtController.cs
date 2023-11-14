using MediatR;
using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.Commands.Court.AddCourt;
using PadelApp.Application.Commands.Court.RemoveCourt;
using PadelApp.Application.Commands.Court.UpdateCourtStatus;
using PadelApp.Application.Queries.Court.CourtById;
using PadelApp.Domain.Enums;
using PadelApp.Presentation.Attributes;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("court")]
public class CourtController : ControllerBase
{
    private readonly IMediator _mediator;

    public CourtController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetCourtById([FromBody] CourtByIdRequest request, CancellationToken cancellationToken)
    {
        var query = new CourtByIdCommand(request.CourtId);
        
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [Token(Role.COURT_OWNER)]
    [HttpPost("add")]
    public async Task<IActionResult> AddCourt([FromBody] AddCourtRequest request, CancellationToken cancellationToken)
    {
        var query = new AddCourtCommand(
            request.Name,
            request.Description,
            request.Address,
            request.WorkStartTime,
            request.WorkEndTime,
            request.Price);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.COURT_OWNER)]
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveCourt([FromBody] RemoveCourtRequest request, CancellationToken cancellationToken)
    {
        var query = new RemoveCourtCommand(request.CourtId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.COURT_OWNER)]
    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateCourtStatus([FromBody] UpdateCourtStatusRequest request, CancellationToken cancellationToken)
    {
        var query = new UpdateCourtStatusCommand(request.CourtId, request.Status);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
}