using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Commands.Booking.RejectBooking;

public class RejectBookingRequestHandler : IRequestHandler<RejectBookingCommand, RejectBookingResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectBookingRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RejectBookingResponse> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByBooking(request.CourtId, request.BookingId);
        
        if (court is null)
            throw new CourtNotFoundException($"Court with id {request.CourtId} not found");
        
        var booking = court.Bookings.FirstOrDefault(b => b.Id == request.BookingId);
        
        if (booking is null)
            throw new BookingNotFoundException($"Booking with id {request.BookingId} not found");
        
        court.RejectBooking(booking, request.BookerId);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RejectBookingResponse(true);
    }
}