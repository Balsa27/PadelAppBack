using Microsoft.EntityFrameworkCore.Storage;

namespace PadelApp.Application.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void LogEntityStates();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}