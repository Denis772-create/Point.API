using FluentValidation.Results;
using Point.Shared;

namespace Point.API.Extensions;

public static class OperationResultExtensions
{
    public static Task<IActionResult> ToResponseAsync<T>(this OperationResult<T> result,
        Func<T, Task<IActionResult>> okFn, Func<ValidationResult, IActionResult> badRequestFn)
    {
        return result.MatchAsync(okFn, validationResult => Task.FromResult(badRequestFn(validationResult)));
    }

    public static IActionResult ToResponse<T>(this OperationResult<T> result, Func<T, IActionResult> okFn,
        Func<ValidationResult, IActionResult> badRequestFn)
    {
        return result.Match(okFn, badRequestFn);
    }
}