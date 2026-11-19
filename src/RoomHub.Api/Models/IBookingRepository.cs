namespace RoomHub.Api.Models;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Booking>> GetForRoomAsync(Guid roomId, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
}