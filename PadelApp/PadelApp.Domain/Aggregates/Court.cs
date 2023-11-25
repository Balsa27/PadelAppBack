using PadelApp.Application.Exceptions;
using PadelApp.Domain.DomainEvents;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Enums;
using PadelApp.Domain.Events.DomainEvents;
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
    public TimeSpan WorkingStartTime { get; private set; }
    public TimeSpan WorkingEndTime { get; private set; }
    public CourtStatus Status { get; private set; }
    public Address Address { get; private set; }
    public List<Booking> Bookings { get; private set; } = new();
    public List<Price> Prices { get; private set; } = new();
    public List<string>? CourtImages { get; private set; } = new();

    public Court()
    {
        
    }
    public Court(
        Guid organizationId,
        string name,
        string description,
        Address address,
        TimeSpan workingStartTime,
        TimeSpan workingEndTime,
        List<Price> prices,
        string? profileImage = null,
        List<string>? courtImages = null)
    {
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        Address = address;
        WorkingStartTime = workingStartTime;
        WorkingEndTime = workingEndTime;
        ProfileImage = profileImage;
        CourtImages = courtImages;
        Status = CourtStatus.Available; // Initializing with Available status
        
        ConstructorPriceValidate(prices);
        RaiseDomainEvent(new CourtCreatedDomainEvent(Id, OrganizationId));
    }
    
    public Court(
        Guid courtId,
        Guid organizationId,
        string name,
        string description,
        Address address,
        TimeSpan workingStartTime,
        TimeSpan workingEndTime,
        CourtStatus status,
        List<Booking> bookings,
        List<Price> prices,
        string? profileImage = null,
        List<string>? courtImages = null)
    {
        Id = courtId;
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        Address = address;
        WorkingStartTime = workingStartTime;
        WorkingEndTime = workingEndTime;
        Status = status;
        Bookings = bookings;
        Prices = prices;
        ProfileImage = profileImage;
        CourtImages = courtImages;
    }
    
    // public Court(
    //     string name,
    //     string description,
    //     Address address,
    //     TimeSpan workingStartTime,
    //     TimeSpan workingEndingTime,
    //     Price prices)
    // {
    //     Name = name;
    //     Description = description;
    //     Address = address;
    //     WorkingStartTime = workingStartTime;
    //     WorkingEndTime = workingEndingTime;
    //     AddCourtPricing(prices);
    // }

    // public Court(
    //     Guid id, 
    //     string name,
    //     string description,
    //     Address address,
    //     DateTime workingStartTime,
    //     DateTime workingEndingTime,
    //     List<Price> prices,
    //     List<Booking> bookings,
    //     List<string>? courtImages,
    //     string? imageUrl,
    //     CourtStatus status,
    //     Guid organizationId)
    // {
    //     Id = id;
    //     Name = name;
    //     Description = description;
    //     Address = address;
    //     WorkingStartTime = workingStartTime;
    //     WorkingEndingTime = workingEndingTime;
    //     Prices = prices;
    //     Bookings = bookings;
    //     CourtImages = courtImages;
    //     ProfileImage = imageUrl;
    //     Status = status;
    //     OrganizationId = organizationId;
    // }
    
    public void UpdateCourtDetails(string name, string description, Address address, TimeSpan start, TimeSpan end)
    {
        Name = name;
        Description = description;
        Address = address;
        WorkingStartTime = start;
        WorkingEndTime = end;
    }
   
    public bool IsAvailable(DateTime startTime, DateTime endTime)
    {
        // Basic validation checks
        if (Status != CourtStatus.Available)
            return false;

        if (startTime == endTime)
            return false;

        if (startTime < DateTime.Now)
            return false;

        if (startTime > endTime)
            return false;

        // Extracting time and day of the week from the booking
        TimeSpan startTimeSpan = startTime.TimeOfDay;
        TimeSpan endTimeSpan = endTime.TimeOfDay;
        

        // Checking if the booking time slot is within court working hours
        if (startTimeSpan < WorkingStartTime || endTimeSpan > WorkingEndTime)
            return false;

        // Checking if the booking duration matches any price duration
        var duration = endTime - startTime;
        var isValidDuration = Prices.Any(p => p.Duration == duration);

        if (!isValidDuration)
            return false;

        // Checking if the booking overlaps with existing bookings
        var overlapsWithExistingBooking = Bookings.Any(b => b.IsOverlapping(startTime, endTime));

        if (overlapsWithExistingBooking)
            return false;

        // Checking if there is a matching price for the booking's day and time
        
        DayOfWeek bookingDayOfWeek = startTime.DayOfWeek;        
        
        var isPriceApplicableAndAvailable = Prices.Any(price =>
            price.Days.Contains(bookingDayOfWeek) &&
            startTimeSpan >= price.TimeStart &&
            endTimeSpan <= price.TimeEnd);

        return isPriceApplicableAndAvailable;
    }

    
    public void CreateBooking(Booking booking)
    {
        booking.ValidateBooking(booking.StartTime, booking.EndTime);
        
        if(!IsAvailable(booking.StartTime, booking.EndTime))
            throw new CourtNotAvailableException("Cannot add booking to court");
        
        Bookings.Add(booking);
        
        RaiseDomainEvent(new BookingCreatedDomainEvent(OrganizationId));
    }
    
    public void AcceptBooking(Booking existingBooking, Guid bookerId)
    {
        if (existingBooking.Status == BookingStatus.Confirmed)
            throw new Exception("Booking is already confirmed");

        existingBooking.Confirm();
        
        RaiseDomainEvent(new BookingAcceptedDomainEvent(existingBooking.Id, bookerId));
    }

    public void CancelBooking(Booking booking)
    {
        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled.");
        
        booking.ChangeBookingStatus(BookingStatus.Cancelled);
        
        //RaiseDomainEvent(new BookingCancelledDomainEvent(booking.Id, booking.BookerId));

        // var nextBookerId = booking.WaitingList.GetNextUser();
        //
        // if (nextBookerId.HasValue)
        // {
        // }
    }

    public void RemovePrices()
    {
        Prices.Clear();
    }

    public void RejectBooking(Booking existingBooking, Guid bookerId)
    {
        if (existingBooking.Status == BookingStatus.Rejected)
            throw new Exception("Booking is already rejected");
        if (existingBooking.Status == BookingStatus.Confirmed)
            throw new Exception("Booking is already confirmed");
            
        existingBooking.Reject();
        
        //RaiseDomainEvent(new BookingRejectedDomainEvent(existingBooking.Id, bookerId));        
    }

    public void RescheduleBooking(Booking existingBooking, DateTime startTime, DateTime endTime)
    {
        if(!IsAvailable(startTime, endTime))
            throw new Exception("Cannot reschedule booking");
        
        existingBooking.RescheduleBooking(startTime, endTime);
    }
    
    // public void CancelBooking(Booking existingBooking)
    // {
    //     existingBooking.Cancel();
    //     Bookings.Remove(existingBooking);
    // }

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

    public void UpdateCourtPricing(Price price)
    {
        var existingPrice = Prices.FirstOrDefault(p => p.Id == price.Id);
        
        if (existingPrice is null)
            throw new PriceNotFoundException("Price not found");

        Prices.Remove(existingPrice);

        try
        {
            ValidatePrice(price);
            existingPrice.Update(price);
        }
        finally
        {
            Prices.Add(existingPrice);
        }
        
        existingPrice.Update(price);
    }

    public void RemoveCourtPricing(Guid priceId)
    {
        var existingPrice = Prices.FirstOrDefault(p => p.Id == priceId);
        
        if (existingPrice is null)
            throw new PriceNotFoundException("Price not found");

        Prices.Remove(existingPrice);
    }
    
    public void AddCourtPricing(Price price)
    {
        ValidatePrice(price);
        Prices.Add(price);
    }
    
    private void ConstructorPriceValidate(List<Price> prices)
    {
        foreach (var newPrice in prices)
        {
            ValidatePrice(newPrice);
            Prices.Add(newPrice);
        }
        
    }
    
    private void ValidatePrice(Price newPrice)
    {
        if(Prices.Count == 0)
            return;
        
        foreach (var existingPrice in Prices)
        {

            if (existingPrice.Id == newPrice.Id)
                continue;
            
            bool hasCommonDay = newPrice.Days != null &&
                                newPrice.Days.Intersect(existingPrice.Days).Any();

            if (hasCommonDay)
            {
                bool overlaps = newPrice.TimeStart < existingPrice.TimeEnd && 
                                newPrice.TimeEnd > existingPrice.TimeStart;

                if (overlaps)
                    throw new Exception("Price time slots overlap on the same day");
            }
        }
    }
    
    public void RemoveCourtFromOrganization(Guid organizationId, Guid courtId) 
        => RaiseDomainEvent(new RemoveCourtFromOrganizationDomainEvent(courtId, organizationId));
}