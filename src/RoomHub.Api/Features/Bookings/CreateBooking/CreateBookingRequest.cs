namespace RoomHub.Api.Features.Bookings.CreateBooking;

public record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);
