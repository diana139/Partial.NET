using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Common;

public interface IQueryHandler<TQuery, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken ct);
}