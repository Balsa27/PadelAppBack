using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Commands.Player.CreateBooking;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Commands.Booking.CreateBooking;

public class CreateBookingRequestHandler : IRequestHandler<CreateBookingCommand, CreateBookingResponse>
{
    private readonly ICourtRepository _courtRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingRequestHandler(ICourtRepository courtRepository, IUnitOfWork unitOfWork)
    {
        _courtRepository = courtRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var court = await _courtRepository.GetCourtByIdAsync(request.CourtId);

        if (court is null)
            throw new CourtNotFoundException("Court not found");

        var booking = new Domain.Entities.Booking(
            request.CourtId,
            request.CortName,
            request.BookerId,
            request.StartTime,
            request.EndTime);
        
        court.CreateBooking(booking);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookingResponse(
            booking.Id,
            booking.CourtId,
            booking.CourtName,
            booking.BookerId,
            booking.StartTime,
            booking.EndTime);
    }
}