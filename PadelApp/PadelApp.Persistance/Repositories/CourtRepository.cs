using Microsoft.EntityFrameworkCore;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Enums;
using PadelApp.Persistance.EFC;

namespace PadelApp.Persistance.Repositories;

public class CourtRepository : ICourtRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourtRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Court?> GetCourtByIdAsync(Guid courtId) //check asnotracking
        => await _dbContext
            .Courts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == courtId);
    
    public async Task AddCourtAsync(Court court) => await _dbContext.Courts.AddAsync(court);

    public void RemoveCourtAsync(Court court)
    {
        if (court == null) throw new ArgumentNullException(nameof(court));
        _dbContext.Courts.Remove(court);
    }

    public async Task<bool> CourtExistsByNameAsync(string name) 
        => await _dbContext.
            Courts
            .AnyAsync(c => c.Name == name);

    public async Task UpdateCourtStatus(Guid courtId, CourtStatus status)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            $"UPDATE Courts SET Status = {0} WHERE Id = {1}",
            (int)status, 
            courtId
        );
    }

    public async Task<Court?> GetCourtByBooking(Guid courtId, Guid bookingId)
    {
        return await _dbContext.Courts
            .Include(c => c.Bookings)
            .Where(c => c.Id == courtId && c.Bookings.Any(b => b.Id == bookingId))
            .FirstOrDefaultAsync();
    }
}