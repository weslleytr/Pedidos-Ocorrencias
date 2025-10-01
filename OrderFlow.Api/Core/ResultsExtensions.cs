using OrderFlow.Application.Core;

namespace OrderFlow.Api.Core
{
    public static class ResultsExtensions
    {
        public static IResult MapResults<T>(this IResultExtensions resultExtensions, Result<T> result)
        {
            if (result.isSuccess)
            {
                return Results.Ok(result.Value);  
            }

            return GetErrorResult(result.Error);
        }

        internal static IResult GetErrorResult(Error error)
        {
            return error.Type switch
            {
                ErrorType.Validation => Results.BadRequest(error),
                ErrorType.Conflict => Results.Conflict(error),
                ErrorType.NotFound => Results.NotFound(error),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.Forbid(),
                _ => Results.Problem(
                        statusCode: 500,
                        title: "Server Failure",
                        type: Enum.GetName(typeof(ErrorType), error.Type),
                        extensions: new Dictionary<string, object?>
                        {
                            { "errors", new[] { error } }
                        })
            };
        }
    }
}
