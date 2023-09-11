using Microsoft.EntityFrameworkCore;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.EFC;
using System;
using System.Threading.Tasks;
using PadelApp.Domain.Aggregates;

namespace PadelApp.Persistance.Repositories;

//FindAsync - optimized for looking at the primary key
//FirstOrDefaultAsync - optimized for looking at other fields
public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PlayerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Player?> GetById(Guid guid)
        => _dbContext.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == guid);

    public async Task<Player?> GetByUsername(string username)
        => await _dbContext.Players.FirstOrDefaultAsync(p => p.Username == username);

    public async Task<Player?> GetByEmail(string email)
        => await _dbContext.Players.FirstOrDefaultAsync(p => p.Email == email);

    public async Task<Player?> GetByUsernameOrEmail(string username, string email)
        => await _dbContext.Players.FirstOrDefaultAsync(p => p.Username == username || p.Email == email);

    public async Task AddAsync(Player player) 
        => await _dbContext.Players.AddAsync(player);
    
}