using RoomHub.Api.Common;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
                Guid id,
                ICommandHandler<CancelBookingCommand, CancelBookingResponse> handler,
                CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(new CancelBookingCommand(id), ct);
                return result.ToNoContent();
            })
            .WithTags("Bookings");
    }
}
