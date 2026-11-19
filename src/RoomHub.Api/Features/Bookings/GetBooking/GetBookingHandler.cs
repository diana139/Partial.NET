using RoomHub.Api.Common;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookings)
    : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<Result<GetBookingResponse>> HandleAsync(GetBookingQuery query, CancellationToken ct)
    {
        var b = await bookings.GetByIdAsync(query.Id, ct);
        if (b is null)
            return Result<GetBookingResponse>.Failure(Error.NotFound("Booking.NotFound", "The booking does not exist."));

        return Result<GetBookingResponse>.Success(new GetBookingResponse(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status.ToString()));
    }
}
