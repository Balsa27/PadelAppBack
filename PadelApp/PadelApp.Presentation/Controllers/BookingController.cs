using MediatR;
using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.Commands.Booking.AcceptBooking;
using PadelApp.Application.Commands.Booking.RejectBooking;
using PadelApp.Application.Commands.Player.CreateBooking;
using PadelApp.Application.Queries.Booking.AllPendingBookings;
using PadelApp.Application.Queries.Booking.BookingById;
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
    
    [Token(Role.USER)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(
            request.CourtId,
            request.CourtName,
            request.BookerId,
            request.StartTime,
            request.EndTime);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.COURT_OWNER)]
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptBooking([FromBody] AcceptBookingRequest request, CancellationToken cancellationToken)
    {
        var command = new AcceptBookingCommand(
            request.BookingId,
            request.BookerId,
            request.CourtId);
        
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [Token(Role.COURT_OWNER)]
    [HttpPost("reject")]
    public async Task<IActionResult> RejectBooking([FromBody] RejectBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectBookingCommand(
            request.BookingId,
            request.BookerId,
            request.CourtId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("get/id")]
    public async Task<IActionResult> GetBookingById([FromBody] BookingByIdRequest request, CancellationToken cancellationToken)
    {
        var command = new BookingByIdCommand(request.BookingId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.COURT_OWNER)]
    [HttpGet("/court-pending")]
    public async Task<IActionResult> GetCourtPendingBookings([FromBody] CourtPendingBookingsCommand request, CancellationToken cancellationToken)
    {
        var command = new CourtPendingBookingsCommand(request.CourtId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.USER)]
    [HttpGet("user-pending")]
    public async Task<IActionResult> GetUserPendingBookings([FromBody] UserPendingBookingsCommand request, CancellationToken cancellationToken)
    {
        var command = new UserPendingBookingsCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Token(Role.USER)]
    [HttpGet("user-upcoming")]
    public async Task<IActionResult> GetUserUpcomingBookings([FromBody] UserUpcomingBookingsCommand request, CancellationToken cancellationToken)
    {
        var command = new UserUpcomingBookingsCommand();
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}