using FluentValidation.Results;
using MediatR;

namespace Point.Shared;

public class OperationResult : OperationResult<Unit>
{
    private OperationResult(ValidationResult validation)
        : base(validation)
    {
    }

    private OperationResult(Unit value)
        : base(value)
    {
    }

    public new static OperationResult Failure(ValidationResult validation)
    {
        return new OperationResult(validation);
    }

    public static OperationResult Success()
    {
        return new OperationResult(Unit.Value);
    }
}

public class OperationResult<TValue> : Either<TValue, ValidationResult>
{
    public bool IsSuccessful => IsLeft;
    public ValidationResult? Validation => Right;
    public TValue? Value => Left;


    public OperationResult(TValue left) : base(left)
    {
    }

    public OperationResult(ValidationResult right) : base(right)
    {
    }

    public static OperationResult<TValue> Failure(ValidationResult validation)
    {
        return new OperationResult<TValue>(validation);
    }

    public static OperationResult<TValue> Success(TValue value)
    {
        return new OperationResult<TValue>(value);
    }

    public OperationResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> mapFn)
    {
        return IsSuccessful
            ? new OperationResult<TNewValue>(mapFn(Value!))
            : new OperationResult<TNewValue>(Validation!);
    }
}