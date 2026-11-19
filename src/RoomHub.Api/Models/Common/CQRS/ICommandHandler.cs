using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Common;

public interface ICommandHandler<TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct);
}
