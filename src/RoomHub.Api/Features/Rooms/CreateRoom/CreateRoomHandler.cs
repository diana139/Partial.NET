using RoomHub.Api.Common;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(IRoomRepository rooms)
    : ICommandHandler<CreateRoomCommand, CreateRoomResponse>
{
    public async Task<Result<CreateRoomResponse>> HandleAsync(CreateRoomCommand command, CancellationToken ct)
    {
        var roomResult = Room.Create(command.Name, command.Capacity);
        if (roomResult.IsFailure)
            return Result<CreateRoomResponse>.Failure(roomResult.Error);

        var room = roomResult.Value;
        await rooms.AddAsync(room, ct);

        return Result<CreateRoomResponse>.Success(new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
