﻿using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Entities;

public class OrganizationCourt : Entity
{
    public Guid OrganizationId { get; init; }
    public Guid CourtId { get; init; }

    public OrganizationCourt(Guid organizationId, Guid courtId)
    {
        OrganizationId = organizationId;
        CourtId = courtId;
    }
}