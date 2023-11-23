using Microsoft.EntityFrameworkCore;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.EFC;

namespace PadelApp.Persistance.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private ApplicationDbContext _dbContext;

    public OrganizationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Organization?> GetByIdAsync(Guid guid) 
        => await _dbContext.Organizations.FindAsync(guid);

    public async Task<Organization?> GetByUsernameOrEmail(string username, string email) 
        => await _dbContext.Organizations
            .FirstOrDefaultAsync(o =>
                o.Username == username || 
                o.Email == email &&
                o.IsActive == true);

    public void Remove(Organization organization) => _dbContext.Organizations.Remove(organization);
    
    public void Update(Organization organization) => _dbContext.Organizations.Update(organization);
    
    public async Task Add(Organization organization) => await _dbContext.Organizations.AddAsync(organization);
    
}