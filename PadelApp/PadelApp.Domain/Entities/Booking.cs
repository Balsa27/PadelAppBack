using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;
using PadelApp.Domain.ValueObjects;
using PadelApp.Persistance.Dbos;

namespace PadelApp.Domain.Entities;

public class Booking : Entity
{
    //todo: potentially add a list of user ids who attend booking
    public Guid CourtId { get; private set; }
    public string CourtName { get; private set; }
    public List<BookingAttendee> Attendees { get; private set; } = new ();
    public WaitingList WaitingList { get; private set; } = new();
    public Guid BookerId { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    
    public Booking(
        Guid courtId,
        string courtName,
        Guid bookerId,
        BookingStatus status,
        DateTime startTime,
        DateTime endTime)
    {
        ValidateBooking(startTime, endTime);
       
        CourtId = courtId;
        CourtName = courtName;
        BookerId = bookerId;
        Status = status;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    public Booking(
        Guid courtId,
        string courtName,
        Guid bookerId,
        DateTime startTime,
        DateTime endTime)
    {
        CourtId = courtId;
        CourtName = courtName;
        BookerId = bookerId;
        StartTime = startTime;
        EndTime = endTime;
        Status = BookingStatus.Pending;
    }
    
    public void AddAttendee(Guid playerId)
    {
        if (Attendees.Any(a => a.PlayerId == playerId))
            throw new InvalidOperationException("User is already attending the booking");

        Attendees.Add(new BookingAttendee { BookingId = this.Id, PlayerId = playerId });
    }
 
    public void Confirm()
    {
        if (Status == BookingStatus.Cancelled)
            throw new Exception("Cannot confirm a cancelled booking");
        if (Status == BookingStatus.Confirmed)
            throw new Exception("Booking is already confirmed");
        Status = BookingStatus.Confirmed;
    }

    public void Reject()
    {
        if(Status == BookingStatus.Cancelled)
            throw new Exception("Cannot reject a cancelled booking");
        if (Status == BookingStatus.Rejected)
            throw new Exception("Booking is already rejected");
        Status = BookingStatus.Rejected;
    }
    
    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled.");
        
        Status = BookingStatus.Cancelled;
        
        WaitingList.NotifyNextUser();
    }
    
    public void RescheduleBooking(DateTime startTime, DateTime endTime) 
    {
        if(Status == BookingStatus.Cancelled)
            throw new Exception("Cannot reschedule a cancelled booking");

        StartTime = startTime;
        EndTime = endTime;
    }

    public void AcceptWaitingList(Guid userId)
    {
        WaitingList.Accept(userId);
    }
    
    public void RejectWaitingList(Guid userId)
    {
        WaitingList.Reject(userId);
        WaitingList.NotifyNextUser();
    }

    public bool IsOverlapping(DateTime start, DateTime end) => StartTime < end && start < EndTime;
    
    private void ValidateBooking(DateTime startTime, DateTime endTime)
    {
        if (startTime == endTime)
            throw new ArgumentException("Start time cannot be the same as end time");
        if (startTime < DateTime.Now)
            throw new ArgumentException("Start time cannot be in the past");
        if (startTime > endTime)
            throw new ArgumentException("Start time cannot be after end time");
    }
}