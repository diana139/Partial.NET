using RoomHub.Api.Common;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (
                Guid roomId,
                CreateBookingRequest request,
                ICommandHandler<CreateBookingCommand, CreateBookingResponse> handler,
                CancellationToken ct) =>
            {
                var command = new CreateBookingCommand(
                    roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);
                var result = await handler.HandleAsync(command, ct);
                return result.ToCreated(b => $"/api/bookings/{b.Id}");
            })
            .WithTags("Bookings");
    }
}
