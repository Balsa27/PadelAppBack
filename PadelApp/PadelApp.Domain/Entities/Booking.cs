﻿using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;
using PadelApp.Domain.ValueObjects;
using PadelApp.Persistance.Dbos;

namespace PadelApp.Domain.Entities;

public class Booking : Entity
{
    public Guid CourtId { get; private set; }
    //public string CourtName { get; private set; }
    //public List<BookingAttendee> Attendees { get; private set; } = new ();
    //public WaitingList WaitingList { get; private set; } = new();
    public Guid BookerId { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    
    
    public Booking(
        Guid id,
        Guid courtId,
        Guid bookerId,
        DateTime startTime,
        DateTime endTime,
        BookingStatus status)
    {
        //ValidateBooking(startTime, endTime);
        Id = id;
        CourtId = courtId;
        BookerId = bookerId;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }
    
    public Booking(
        Guid courtId,
        Guid bookerId,
        DateTime startTime,
        DateTime endTime)
    {
        //ValidateBooking(startTime, endTime);
       
        CourtId = courtId;
        BookerId = bookerId;
        StartTime = startTime;
        EndTime = endTime;
        Status = BookingStatus.Pending;
    }
    
    // public    void AddAttendee(Guid playerId)
    // {
    //     if (Attendees.Any(a => a.PlayerId == playerId))
    //         throw new InvalidOperationException("User is already attending the booking");
    //
    //     Attendees.Add(new BookingAttendee { BookingId = this.Id, PlayerId = playerId });
    // }
 
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
        //WaitingList.Accept(userId);
    }
    
    public void RejectWaitingList(Guid userId)
    {
        // WaitingList.Reject(userId);
        // WaitingList.NotifyNextUser();
    }

    public void ChangeBookingStatus(BookingStatus status)
    {
        Status = status;
    }

    public bool IsOverlapping(DateTime start, DateTime end) => StartTime < end && start < EndTime || Status == BookingStatus.Confirmed;
    
    public void ValidateBooking(DateTime startTime, DateTime endTime)
    {
        if (startTime == endTime)
            throw new ArgumentException("Start time cannot be the same as end time");
        if (startTime < DateTime.Now)
                throw new ArgumentException("Start time cannot be in the past");
        if (startTime > endTime)
            throw new ArgumentException("Start time cannot be after end time");
    }
}