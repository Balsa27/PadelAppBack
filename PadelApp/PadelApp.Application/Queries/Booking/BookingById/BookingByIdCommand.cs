using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Booking.BookingById;

public record BookingByIdCommand(Guid BookingId) : IRequest<BookingByIdResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
