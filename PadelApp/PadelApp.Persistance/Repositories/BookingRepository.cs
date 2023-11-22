using Microsoft.EntityFrameworkCore;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Enums;
using PadelApp.Persistance.EFC;

namespace PadelApp.Persistance.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BookingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
        => await _dbContext.Courts
            .Where(c => c.Bookings.Any(b => b.Id == bookingId))
            .SelectMany(c => c.Bookings)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

    public async Task<List<Booking>?> GetUserUpcomingBookingAsync(Guid userId)
    {
        var currentTime = DateTime.UtcNow;

        var bookings = await _dbContext.Courts
            .Where(c => c.Bookings.Any(b
                => b.BookerId == userId &&
                   b.StartTime > currentTime))
            .SelectMany(c => c.Bookings)
            .Where(b =>
                b.BookerId == userId &&
                b.Status == BookingStatus.Confirmed &&
                b.EndTime >= currentTime)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
        
        return bookings.Count == 0 ? null : bookings;
    }

    public async Task<List<Booking>?> GetUserPendingBookingAsync(Guid userId)
    {
        var currentTime = DateTime.UtcNow;

        var bookings = await _dbContext.Courts
            .Where(c => c.Bookings.Any(b
                => b.BookerId == userId &&
                   b.StartTime > currentTime))
            .SelectMany(c => c.Bookings)
            .Where(b =>
                b.BookerId == userId &&
                b.Status == BookingStatus.Pending &&
                b.EndTime >= currentTime)
            .OrderBy(b => b.StartTime)
            .ToListAsync();   
        
        return bookings.Count == 0 ? null : bookings; 
    }

    public async Task<List<Booking>?> GetCourtPendingBookingsAsync(Guid courtId)
    {
        var currentTime = DateTime.UtcNow;
        
        var bookings = await _dbContext.Courts
            .Where(c => c.Id == courtId)
            .SelectMany(c => c.Bookings)
            .Where(b => 
                b.Status == BookingStatus.Pending &&
                b.EndTime >= currentTime)
            .OrderBy(b => b.StartTime)
            .ToListAsync();

        return bookings.Count == 0 ? null : bookings;
    }

    public async Task<List<Booking>?> GetCourtUpcomingBookingsAsync(Guid courtId)
    {
        var currentTime = DateTime.UtcNow;
        
        var bookings = await _dbContext.Courts
            .Where(c => c.Id == courtId)
            .SelectMany(c => c.Bookings)
            .Where(b => 
                b.Status == BookingStatus.Confirmed &&
                b.EndTime >= currentTime)
            .OrderBy(b => b.StartTime)
            .ToListAsync();

        return bookings.Count == 0 ? null : bookings;
    }
}