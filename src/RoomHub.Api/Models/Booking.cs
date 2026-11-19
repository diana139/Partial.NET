using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Models;

public enum BookingStatus
{
    Active,
    Cancelled
}

public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking(Guid id, Guid roomId, string bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
        Status = BookingStatus.Active;
    }

    public static Result<Booking> Create(
        Room room, string? bookedBy, DateTime startUtc, DateTime endUtc, int attendees,
        DateTime nowUtc, IEnumerable<Booking> existingBookings)
    {
        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Result<Booking>.Failure(Error.Validation("Booking.InvalidBookedBy", "BookedBy is required."));

        // R3
        if (endUtc <= startUtc)
            return Result<Booking>.Failure(Error.Validation("Booking.InvalidRange", "End must be after start."));

        // R4
        if (attendees <= 0 || attendees > room.Capacity)
            return Result<Booking>.Failure(Error.Validation("Booking.InvalidAttendees", "Attendees must be between 1 and room capacity."));

        // R5 
        if (startUtc < nowUtc)
            return Result<Booking>.Failure(Error.Validation("Booking.InPast", "Booking cannot start in the past."));

        // R6 
        if (endUtc - startUtc > TimeSpan.FromHours(4))
            return Result<Booking>.Failure(Error.Validation("Booking.TooLong", "Maximum duration is 4 hours."));

        // R7
        foreach (var other in existingBookings)
        {
            if (other.Status == BookingStatus.Active &&
                other.StartUtc < endUtc && startUtc < other.EndUtc)
            {
                return Result<Booking>.Failure(Error.Conflict("Booking.RoomOccupied", "The room is occupied in this interval."));
            }
        }

        return Result<Booking>.Success(new Booking(Guid.NewGuid(), room.Id, bookedBy.Trim(), startUtc, endUtc, attendees));
    }

    // R8
    public Result<Booking> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Result<Booking>.Failure(Error.Conflict("Booking.AlreadyCancelled", "The booking is already cancelled."));

        Status = BookingStatus.Cancelled;
        return Result<Booking>.Success(this);
    }
}

