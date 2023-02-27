using FluentValidation.Results;

namespace Point.Domain.Exceptions;

public static class ValidationErrors
{
    public static ValidationResult DoesNotExist(string entityName)
    {
        return new ValidationResult(
            new[]
            {
                new ValidationFailure("", $"{entityName} doesn't exist.")
            });
    }
}