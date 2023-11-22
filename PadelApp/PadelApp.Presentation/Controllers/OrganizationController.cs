using MediatR;
using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.Commands.Organization.RemoveOrganization;
using PadelApp.Application.Commands.Organization.UpdateOrganization;
using PadelApp.Domain.Enums;
using PadelApp.Presentation.Attributes;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("api/organization")]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Token(Role.Organization)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateOrganization([FromBody] UpdateOrganizationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateOrganizationCommand(
            request.Name,
            request.Description,
            request.Address,
            request.ContactInfo,
            request.WorkingStartHours,
            request.WorkingEndingHours);
        
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteOrganization(CancellationToken cancellationToken)
    {
        var command = new RemoveOrganizationCommand();
        
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}


