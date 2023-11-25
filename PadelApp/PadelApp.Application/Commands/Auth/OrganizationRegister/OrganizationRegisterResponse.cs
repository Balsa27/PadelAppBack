using PadelApp.Domain.Enums;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Organization.OrganizationRegister;

public record OrganizationRegisterResponse(
    Guid OrganizationId, 
    string Username,
    string Email,
    string Name,
    string Description,
    Address Address,
    ContactInfo? ContactInfo,
    TimeSpan Start,
    TimeSpan End,
    OrganizationStatus Status,
    Role Role,
    string Token);