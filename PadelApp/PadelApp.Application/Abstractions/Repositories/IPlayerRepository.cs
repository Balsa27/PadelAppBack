using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;

namespace PadelApp.Application.Abstractions.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetById(Guid guid);
    Task<Player?> GetByUsername(string username);
    Task<Player?> GetByEmail(string email);
    Task<Player?> GetByUsernameOrEmail(string username, string email);
    Task AddAsync(Player player);
    Task<Player?> GetByGoogleId(string googleId);
    Task<Player?> GetByAppleId(string appleId);
}