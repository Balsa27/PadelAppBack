using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Queries.Booking.BookingById;

public class BookingByIdRequestHandler : IRequestHandler<BookingByIdCommand, BookingByIdResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookingByIdRequestHandler(IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BookingByIdResponse> Handle(BookingByIdCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);
        
        if (booking is null)
            throw new BookingNotFoundException("Booking not found");
        
        return new BookingByIdResponse(
            booking.Id,
            booking.CourtId,
            booking.BookerId,
            booking.StartTime,
            booking.EndTime,
            booking.Status);
    }
}