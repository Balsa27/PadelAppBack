using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Booking.AcceptBooking;

public class AcceptBookingRequestHandler : IRequestHandler<AcceptBookingCommand, AcceptBookingResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptBookingRequestHandler(IUnitOfWork unitOfWork, ICourtRepository courtRepository)
    {
        _unitOfWork = unitOfWork;
        _courtRepository = courtRepository;
    }

    public async Task<AcceptBookingResponse> Handle(AcceptBookingCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByBooking(request.CourtId, request.BookingId);
        
        if (court is null)
            throw new CourtNotFoundException($"Court with id {request.CourtId} not found");
        
        var booking = court.Bookings.FirstOrDefault(b => b.Id == request.BookingId);

        if (booking is null)
            throw new BookingNotFoundException($"Booking with id {request.BookingId} not found");
        
        court.AcceptBooking(booking, booking.BookerId);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AcceptBookingResponse(HandlerStrings.BookingAccepted);
    }
}