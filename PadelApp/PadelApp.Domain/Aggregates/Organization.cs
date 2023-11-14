using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Enums;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Domain.Entities;

public class Organization : User
{
    public string Name { get ; private set; }
    public string Description { get; private set; }
    public Address Address { get; private set; }
    public ContactInfo? ContactInfo { get; private set; }
    public DateTime? WorkingStartHours { get; private set; }
    public DateTime? WorkingEndingHours { get; private set; }
    public OrganizationStatus Status { get; private set; }
    public List<OrganizationCourt> Courts { get; private set; } = new();

    public Organization()
    {
        
    }
    
    public Organization(
        string name,
        string description,
        Address address,
        ContactInfo? contactInfo,
        DateTime start,
        DateTime end)
    {
        Name = name;
        Description = description;
        Address = address;
        ContactInfo = contactInfo;
        WorkingStartHours = start;
        WorkingEndingHours = end;
    }

    public Organization( string name,
        string description,
        Address address,
        ContactInfo? contactInfo,
        DateTime start,
        DateTime end,
        OrganizationStatus status,
        List<OrganizationCourt> courts,
        Guid id)
    {
        Id = id;
        Name = name;
        Description = description;
        Address = address;
        ContactInfo = contactInfo;
        WorkingStartHours = start;
        WorkingEndingHours = end;
        Status = status;
        Courts = courts;
    }
   
    public void UpdateCourtDetails(
        string? name = null,
        string? description = null,
        Address? address = null,
        ContactInfo? contactInfo = null,
        DateTime? start = null,
        DateTime? end = null)
    {
        if (name is not null)
            Name = name;
        if (description is not null)
            Description = description;
        if (address is not null)
            Address = address;
        if (contactInfo is not null)
            ContactInfo = contactInfo;
    
        if (start is not null 
            && end is not null)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time");
        }
        
        if (start is not null)
            WorkingStartHours = start;
        if (end is not null)
            WorkingEndingHours = end;
    }

   public void AddCourt(Guid courtId)
   {   
       if (Courts.Any(oc => oc.CourtId == courtId))
           throw new InvalidOperationException("Court is already in the organization");

       var organizationCourt = new OrganizationCourt
       {
           OrganizationId = this.Id,
           CourtId = courtId
       };

       Courts.Add(organizationCourt);
   }
    
   public void RemoveCourt(Guid courtId)
   {
       var organizationCourt = Courts.FirstOrDefault(oc => oc.CourtId == courtId);
       
       if (organizationCourt is null)
           throw new InvalidOperationException("Court not found in the organization");

       Courts.Remove(organizationCourt);
   }

    
    public void Activate() => Status = OrganizationStatus.Active;
    public void Deactivate() => Status = OrganizationStatus.Inactive;
}