using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Repositories;

public sealed class EfRoomRepository(AppDbContext context) : IRoomRepository
{
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task AddAsync(Room room, CancellationToken ct = default) =>
        await context.Rooms.AddAsync(room, ct);
}
