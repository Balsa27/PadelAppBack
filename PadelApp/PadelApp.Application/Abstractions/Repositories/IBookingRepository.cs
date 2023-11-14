﻿using PadelApp.Domain.Entities;

namespace PadelApp.Application.Abstractions.Repositories;

public interface IBookingRepository
{
    public Task<Booking?> GetBookingByIdAsync(Guid bookingId);
    public Task<List<Booking>?> GetUserUpcomingBookingAsync(Guid userId);
    public Task<List<Booking>?> GetUserPendingBookingAsync(Guid userId);
    public Task<List<Booking>?> GetCourtPendingBookingsAsync(Guid courtId);
}