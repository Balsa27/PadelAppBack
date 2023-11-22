using PadelApp.Domain.Entities;

namespace PadelApp.Application.Abstractions.Repositories;

public interface IOrganizationRepository 
{
    public Task<Organization?> GetByIdAsync(Guid guid);
    public Task<Organization?> GetByUsernameOrEmail(string username, string email);
    public void Update(Organization organization);
    public Task Add(Organization organization);
    public void Remove(Organization organization);
    // public void AddCourt(OrganizationCourt organizationCourt);
    // public void RemoveCourt(Guid organizationId, Guid courtId);
}