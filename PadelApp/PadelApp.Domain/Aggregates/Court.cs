using PadelApp.Application.Exceptions;
using PadelApp.Domain.DomainEvents;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Enums;
using PadelApp.Domain.Events.DomainEvents.DomainEventConverter;
using PadelApp.Domain.Primitives;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Domain.Aggregates;

public class Court : AggregateRoot
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string? ProfileImage { get; private set; }
    public DateTime WorkingStartTime { get; private set; }
    public DateTime WorkingEndingTime { get; private set; }
    public CourtStatus Status { get; private set; }
    public Address Address { get; private set; }
    public List<Booking> Bookings { get; private set; } = new();
    public List<Price> Prices { get; private set; } = new();
    public List<string>? CourtImages { get; private set; } = new();

    public Court()
    {
        
    }
    
    public Court(
        string name,
        string description,
        Address address,
        DateTime workingStartTime,
        DateTime workingEndingTime,
        Price prices)
    {
        Name = name;
        Description = description;
        Address = address;
        WorkingStartTime = workingStartTime;
        WorkingEndingTime = workingEndingTime;
        AddCourtPricing(prices);
    }

    public Court(
        Guid id, 
        string name,
        string description,
        Address address,
        DateTime workingStartTime,
        DateTime workingEndingTime,
        List<Price> prices,
        List<Booking> bookings,
        List<string>? courtImages,
        string? imageUrl,
        CourtStatus status,
        Guid organizationId)
    {
        Id = id;
        Name = name;
        Description = description;
        Address = address;
        WorkingStartTime = workingStartTime;
        WorkingEndingTime = workingEndingTime;
        Prices = prices;
        Bookings = bookings;
        CourtImages = courtImages;
        ProfileImage = imageUrl;
        Status = status;
        OrganizationId = organizationId;
    }
    
    public Court(string name, string description, Address address, Price price,
        string? imageUrl = null, List<string>? courtImages = null)
    {
        Name = name;
        Description = description;
        Address = address;
        ProfileImage = imageUrl;
        CourtImages = courtImages;
        AddCourtPricing(price);
    }
    
    public void UpdateCourtDetails(
        string? name = null,
        string? description = null,
        Address? address = null)
    {
        if(name is not null)
            Name = name;
        if(description is not null)
            Description = description;
        if(address is not null)
            Address = address;
    }

    public bool IsAvailable(DateTime startTime, DateTime endTime)
    {
        if(Status != CourtStatus.Available)
            return false;
        if(startTime == endTime)
            return false;
        if(startTime < DateTime.Now)
            return false;
        if (startTime > endTime)
            return false;
        
        if (startTime < WorkingStartTime ||
            endTime > WorkingEndingTime)
            return false;
        
        var duration = endTime - startTime;
        
        var isValidDuration = Prices.Any(p => p.Duration == duration);
        
        if(!isValidDuration)
            return false;

        var overlaps = Bookings.Any(b => b.IsOverlapping(startTime, endTime));

        if (overlaps)
            return false;

        var isValidTimeSlot = Prices.Any(price => 
            (price.TimeStart == null || startTime.TimeOfDay >= price.TimeStart) && 
            (price.TimeEnd == null || endTime.TimeOfDay <= price.TimeEnd));

        var blocksExistingTimeSlot = Prices.Any(price => 
            startTime.TimeOfDay < price.TimeStart && endTime.TimeOfDay > price.TimeEnd);

        if (blocksExistingTimeSlot)
            return false;

        return isValidTimeSlot;
    }
    
    public void CreateBooking(Booking booking)
    {
        if(!IsAvailable(booking.StartTime, booking.EndTime))
            throw new CourtNotAvailableException("Cannot add booking to court");
        
        Bookings.Add(booking);
    }
    
    public void AcceptBooking(Booking existingBooking, Guid bookerId)
    {
        if (existingBooking.Status == BookingStatus.Confirmed)
            throw new Exception("Booking is already confirmed");
        
        existingBooking.Confirm();
        
        RaiseDomainEvent(new BookingAcceptedDomainEvent(existingBooking.Id, bookerId));
    }

    public void RejectBooking(Booking existingBooking, Guid bookerId)
    {
        if (existingBooking.Status == BookingStatus.Rejected)
            throw new Exception("Booking is already rejected");
        
        existingBooking.Reject();
        
        RaiseDomainEvent(new BookingRejectedDomainEvent(existingBooking.Id, bookerId));        
    }

    public void RescheduleBooking(Booking existingBooking, DateTime startTime, DateTime endTime)
    {
        if(!IsAvailable(startTime, endTime))
            throw new Exception("Cannot reschedule booking");
        
        existingBooking.RescheduleBooking(startTime, endTime);
    }
    
    public void CancelBooking(Booking existingBooking)
    {
        existingBooking.Cancel();
        Bookings.Remove(existingBooking);
    }

    public void UpdateCourtStatus(CourtStatus status)
    {
        if (Status == status)
            throw new Exception("Cannot update to the same court status.");
        
        var currentTime = DateTime.UtcNow;
       
        var ongoingBookings = Bookings.Any(
            b => b.Status != BookingStatus.Cancelled &&
                 currentTime >= b.StartTime &&
                 currentTime <= b.EndTime);

        if (ongoingBookings && status != CourtStatus.Closed)
            throw new Exception("Cannot change status; ongoing bookings are present.");

        if (ongoingBookings && 
            status is CourtStatus.Closed or CourtStatus.UnderMaintenance)
        {
            var latestOnGoingBookingEndTime = Bookings
                .Where(b => 
                    b.Status != BookingStatus.Cancelled 
                    && currentTime <= b.EndTime)
                .Max(b => b.EndTime);
            
            RaiseDomainEvent(new CourtStatusChangeDomainEvent(this.Id, latestOnGoingBookingEndTime, status));

            Bookings.Where(b => 
                    b.Status != BookingStatus.Cancelled && 
                    b.StartTime > latestOnGoingBookingEndTime)
                .ToList()
                .ForEach(b => b.Cancel());
            
            return;
        }   
        
        if (!ongoingBookings && 
                 status is CourtStatus.Closed or CourtStatus.UnderMaintenance)
        {
            Status = status;
            return;
        }

        Status = status;
    }

    public void AddCourtPricing(Price price)
    {
        ValidatePrice(price);
        Prices.Add(price);
    }

    public void UpdatePrice(List<Price> price)
    {
        Prices.ForEach(ValidatePrice);
        Prices = price;
    }
    
    private void ValidatePrice(Price newPrice)
    {
        foreach (var existingPrice in Prices)
        {
            if (newPrice.Day == existingPrice.Day ||
                newPrice.Day == null || 
                existingPrice.Day == null)
                if (newPrice.TimeStart < existingPrice.TimeEnd && 
                    newPrice.TimeEnd > existingPrice.TimeStart)
                    throw new Exception("Price time slots overlap");
        }
    }
}