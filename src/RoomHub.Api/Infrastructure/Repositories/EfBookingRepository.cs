using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Repositories;

public sealed class EfBookingRepository(AppDbContext context) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);

    public Task<List<Booking>> GetForRoomAsync(Guid roomId, CancellationToken ct = default) =>
        context.Bookings.Where(b => b.RoomId == roomId).ToListAsync(ct);

    public async Task AddAsync(Booking booking, CancellationToken ct = default) =>
        await context.Bookings.AddAsync(booking, ct);
}
