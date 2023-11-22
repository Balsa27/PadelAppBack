using MediatR;
using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.Commands.Booking.AcceptBooking;
using PadelApp.Application.Commands.Booking.RejectBooking;
using PadelApp.Application.Commands.Player.CreateBooking;
using PadelApp.Application.Queries.Booking.AllPendingBookings;
using PadelApp.Application.Queries.Booking.BookingById;
using PadelApp.Application.Queries.Booking.CourtUpcommingBookings;
using PadelApp.Application.Queries.Booking.UserPendingBookings;
using PadelApp.Application.Queries.Booking.UserUpcomingBookings;
using PadelApp.Domain.Enums;
using PadelApp.Presentation.Attributes;

namespace PadelApp.Presentation.Controllers;

[ApiController]
[Route("api/booking")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Token(Role.Player)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(
            request.CourtId,
            request.BookerId,
            request.StartTime,
            request.EndTime);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptBooking([FromBody] AcceptBookingRequest request, CancellationToken cancellationToken)
    {
        var command = new AcceptBookingCommand(
            request.BookingId,
            request.CourtId);
        
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [Token(Role.Organization)]
    [HttpPost("reject")]
    public async Task<IActionResult> RejectBooking([FromBody] RejectBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectBookingCommand(
            request.BookingId,
            request.CourtId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(Guid id, CancellationToken cancellationToken)
    {
        var command = new BookingByIdCommand(id);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Organization)]
    [HttpGet("court-pending/{id}")]
    public async Task<IActionResult> GetCourtPendingBookings(Guid id, CancellationToken cancellationToken)
    {
        var command = new CourtPendingBookingsCommand(id);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.Player)]
    [HttpGet("user-pending")]
    public async Task<IActionResult> GetUserPendingBookings(CancellationToken cancellationToken)
    {
        var command = new UserPendingBookingsCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("user-upcoming")]
    public async Task<IActionResult> GetUserUpcomingBookings(CancellationToken cancellationToken)
    {
        var command = new UserUpcomingBookingsCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("court-upcoming/{id}")]
    public async Task<IActionResult> GetCourtUpcomingBookings(Guid id, CancellationToken cancellationToken)
    {
        var command = new CourtUpcomingBookingsCommand(id);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}