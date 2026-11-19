using RoomHub.Api.Common;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(IBookingRepository bookings)
    : ICommandHandler<CancelBookingCommand, CancelBookingResponse>
{
    public async Task<Result<CancelBookingResponse>> HandleAsync(CancelBookingCommand command, CancellationToken ct)
    {
        var booking = await bookings.GetByIdAsync(command.Id, ct);
        if (booking is null)
            return Result<CancelBookingResponse>.Failure(Error.NotFound("Booking.NotFound", "The booking does not exist."));

        // R8 este in domeniu (Booking.Cancel)
        var cancelResult = booking.Cancel();
        if (cancelResult.IsFailure)
            return Result<CancelBookingResponse>.Failure(cancelResult.Error);

        return Result<CancelBookingResponse>.Success(
            new CancelBookingResponse(booking.Id, booking.Status.ToString()));
    }
}
