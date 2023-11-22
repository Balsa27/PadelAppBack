using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;

namespace PadelApp.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string GeneratePlayerToken(Player player);
    string GenerateOrganizationToken(Organization organization);
    void InvalidateToken(string token);
}