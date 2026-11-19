namespace RoomHub.Api.Models;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Room room, CancellationToken ct = default);
}