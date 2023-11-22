using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Enums;

namespace PadelApp.Application.Abstractions.Repositories;

public interface ICourtRepository 
{
    public Task<Court?> GetCourtByIdAsync(Guid courtId);
    public Task<Court?> GetCourtAsync(Guid courtId);
    
    public Task AddCourtAsync(Court court);
    public void Update(Court court);
    public void RemoveCourtAsync(Court court);
    public Task<bool> CourtExistsByNameAsync(string name);
    public Task UpdateCourtStatus(Guid courtId, CourtStatus status);
    public Task<Court?> GetCourtByBooking(Guid courtId, Guid bookingId);
    public Task<List<Court>?> GetOrganizationCourts(Guid organizationId); 
}