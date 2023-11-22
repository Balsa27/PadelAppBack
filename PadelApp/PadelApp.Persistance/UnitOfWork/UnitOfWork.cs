using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Primitives;
using PadelApp.Persistance.EFC;
using PadelApp.Persistance.Outbox;
using PadelApp.Persistance.Repositories;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace PadelApp.Persistance.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IPlayerRepository Players { get; private set; }
    public ICourtRepository Courts { get; private set; }
    public IBookingRepository Bookings { get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Players = new PlayerRepository(_dbContext);
        Courts = new CourtRepository(_dbContext);
        Bookings = new BookingRepository(_dbContext);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public void LogEntityStates()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()}");
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}
