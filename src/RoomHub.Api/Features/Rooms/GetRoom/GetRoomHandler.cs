using RoomHub.Api.Common;
using RoomHub.Api.Models;
using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(IRoomRepository rooms)
    : IQueryHandler<GetRoomQuery, GetRoomResponse>
{
    public async Task<Result<GetRoomResponse>> HandleAsync(GetRoomQuery query, CancellationToken ct)
    {
        var room = await rooms.GetByIdAsync(query.Id, ct);
        if (room is null)
            return Result<GetRoomResponse>.Failure(Error.NotFound("Room.NotFound", "The room does not exist."));

        return Result<GetRoomResponse>.Success(new GetRoomResponse(room.Id, room.Name, room.Capacity));
    }
}
