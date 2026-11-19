using RoomHub.Api.Common;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(CreateBookingCommand command, CancellationToken ct)
    {
        // 404 inainte de orice validare (contract)
        var room = await rooms.GetByIdAsync(command.RoomId, ct);
        if (room is null)
            return Result<CreateBookingResponse>.Failure(Error.NotFound("Room.NotFound", "The room does not exist."));

        var existing = await bookings.GetForRoomAsync(room.Id, ct);
        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        // Regulile R2-R7 sunt in domeniu (Booking.Create)
        var bookingResult = Booking.Create(
            room, command.BookedBy, command.StartUtc, command.EndUtc, command.Attendees, nowUtc, existing);

        if (bookingResult.IsFailure)
            return Result<CreateBookingResponse>.Failure(bookingResult.Error);

        var booking = bookingResult.Value;
        await bookings.AddAsync(booking, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<CreateBookingResponse>.Success(
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}
