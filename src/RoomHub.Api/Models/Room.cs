using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Models;

public class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    private Room(Guid id, string name, int capacity)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
    }

    public static Result<Room> Create(string? name, int capacity)
    {
        // R1
        if (string.IsNullOrWhiteSpace(name))
            return Result<Room>.Failure(Error.Validation("Room.InvalidName", "Name is required."));

        if (capacity <= 0)
            return Result<Room>.Failure(Error.Validation("Room.InvalidCapacity", "Capacity must be strictly positive."));

        return Result<Room>.Success(new Room(Guid.NewGuid(), name.Trim(), capacity));
    }
}