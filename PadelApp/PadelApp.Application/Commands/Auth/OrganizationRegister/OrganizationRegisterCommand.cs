using MediatR;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Organization.OrganizationRegister;

public record OrganizationRegisterCommand(
    string Username,
    string Email,
    string Password,
    string Name,
    string Description,
    Address Address,
    ContactInfo? ContactInfo,
    TimeSpan Start,
    TimeSpan End) : IRequest<OrganizationRegisterResponse>;
