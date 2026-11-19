namespace RoomHub.Api.Features.Bookings.CreateBooking;

public record CreateBookingResponse(Guid Id, Guid RoomId, string Status);
