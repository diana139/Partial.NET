using RoomHub.Api.Common;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public static void MapCreateRoom(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", async (
                CreateRoomRequest request,
                ICommandHandler<CreateRoomCommand, CreateRoomResponse> handler,
                CancellationToken ct) =>
            {
                var command = new CreateRoomCommand(request.Name, request.Capacity);
                var result = await handler.HandleAsync(command, ct);
                return result.ToCreated(r => $"/api/rooms/{r.Id}");
            })
            .WithTags("Rooms");
    }
}
