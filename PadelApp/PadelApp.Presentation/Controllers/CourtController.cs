using MediatR;
using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.Commands.Court.AddCourt;
using PadelApp.Application.Commands.Court.AddCourtPrice;
using PadelApp.Application.Commands.Court.RemoveCourt;
using PadelApp.Application.Commands.Court.RemoveCourtPrice;
using PadelApp.Application.Commands.Court.UpdateCourt;
using PadelApp.Application.Commands.Court.UpdateCourtPrice;
using PadelApp.Application.Commands.Court.UpdateCourtStatus;
using PadelApp.Application.Queries.Court.CourtById;
using PadelApp.Application.Queries.Court.GetOrganizationCourts;
using PadelApp.Domain.Enums;
using PadelApp.Presentation.Attributes;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("api/court")]
public class CourtController : ControllerBase
{
    private readonly IMediator _mediator;

    public CourtController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{courtId}")]
    public async Task<IActionResult> GetCourtById(Guid courtId, CancellationToken cancellationToken)
    {
        var query = new CourtByIdCommand(courtId);
        
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [Token(Role.Organization)]
    [HttpPost("add")]
    public async Task<IActionResult> AddCourt([FromBody] AddCourtRequest request, CancellationToken cancellationToken)
    {
        var query = new AddCourtCommand(
            request.Name,
            request.Description,
            request.Address,
            request.WorkStartTime,
            request.WorkEndTime,
            request.Prices,
            request.ImageUrl,
            request.CourtImages);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveCourt([FromBody] RemoveCourtRequest request, CancellationToken cancellationToken)
    {
        var query = new RemoveCourtCommand(request.CourtId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateCourtStatus([FromBody] UpdateCourtStatusRequest request, CancellationToken cancellationToken)
    {
        var query = new UpdateCourtStatusCommand(request.CourtId, request.Status);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("organization/{organizationId}")]
    public async Task<IActionResult> GetOrganizationCourts(Guid organizationId, CancellationToken cancellationToken)
    {
        var query = new OrganizationCourtsCommand(organizationId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCourt([FromBody] UpdateCourtRequest request, CancellationToken cancellationToken)
    {
        var query = new UpdateCourtCommand(
            request.CourtId,
            request.Name,
            request.Description,
            request.Address,
            request.WorkStartTime,
            request.WorkEndTime);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpPut("update/price")]
    public async Task<IActionResult> UpdateCourtPrice([FromBody] UpdateCourtPriceRequest request, CancellationToken cancellationToken)
    {
        var query = new UpdateCourtPriceCommand(request.CourtId, request.Price);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpDelete("remove/price")]
    public async Task<IActionResult> RemoveCourtPrice([FromBody] RemoveCourtPriceRequest request, CancellationToken cancellationToken)
    {
        var query = new RemoveCourtPriceCommand(request.CourtId, request.PriceId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpPost("add/price")]
    public async Task<IActionResult> AddCourtPrice([FromBody] AddCourtPriceRequest request, CancellationToken cancellationToken)
    {
        var query = new AddCourtPriceCommand(
            request.CourtId, 
            request.amount,
            request.duration,
            request.startTime,
            request.endTime,
            request.daysOfWeek
            );
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
}