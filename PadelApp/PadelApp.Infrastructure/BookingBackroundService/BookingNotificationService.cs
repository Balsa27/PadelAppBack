using Microsoft.Extensions.Hosting;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Entities;

namespace PadelApp.Infrastructure.BookingBackroundService;

public class BookingNotificationService : BackgroundService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly INotificationService _notificationService;

    public BookingNotificationService(INotificationService notificationService, IBookingRepository bookingRepository)
    {
        _notificationService = notificationService;
        _bookingRepository = bookingRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var bookingsToCheck = await _bookingRepository.GetBookingsToCheck();

            if (bookingsToCheck != null)
            {
                foreach (var booking in bookingsToCheck)
                {
                    if (ShouldNotifyNextUser(booking))
                    {
                        var nextUserId = booking.WaitingList.GetNextUser();

                        if (nextUserId.HasValue)
                        {
                            await _notificationService.NotifyUserForBookingAcceptance(nextUserId.Value, booking.Id);
                            booking.WaitingList.UserIds.Remove(nextUserId.Value);
                            
                        }
                    }
                }
            }
        }
        
        await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
    }
    
    private bool ShouldNotifyNextUser(Booking booking) => booking.WaitingList.UserIds.Any();
}