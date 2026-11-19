using RoomHub.Api.Models.Common;

namespace RoomHub.Api.Common.Http;

// Traduce Result<T> / Error în răspunsuri HTTP (singurul loc care știe de status codes)
public static class ResultHttpMapper
{
    public static IResult ToOk<T>(this Result<T> result) =>
        result.IsSuccess ? Results.Ok(result.Value) : result.Error.ToProblem();

    public static IResult ToCreated<T>(this Result<T> result, Func<T, string> location) =>
        result.IsSuccess ? Results.Created(location(result.Value), result.Value) : result.Error.ToProblem();

    public static IResult ToNoContent<T>(this Result<T> result) =>
        result.IsSuccess ? Results.NoContent() : result.Error.ToProblem();

    // Error -> ProblemDetails cu codul HTTP potrivit
    public static IResult ToProblem(this Error error)
    {
        if (error.Type == ErrorType.Validation && error.FieldErrors is { Count: > 0 })
        {
            return Results.ValidationProblem(
                errors: error.FieldErrors.ToDictionary(kv => kv.Key, kv => kv.Value),
                title: error.Code,
                detail: error.Message);
        }

        return Results.Problem(
            title: error.Code,
            detail: error.Message,
            statusCode: error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            });
    }
}