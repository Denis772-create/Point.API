namespace Point.Shared;

public class Either<TLeft, TRight>
{
    public TLeft? Left { get; }
    public TRight? Right { get; }

    public bool IsLeft { get; }

    public Either(TLeft left)
    {
        Left = left;
        IsLeft = true;
    }

    public Either(TRight right)
    {
        Right = right;
        IsLeft = false;
    }

    public T Match<T>(Func<TLeft, T> left, Func<TRight, T> right)
    {
        return IsLeft ? left(Left!) : right(Right!);
    }

    public Task<T> MatchAsync<T>(Func<TLeft, Task<T>> left, Func<TRight, Task<T>> right)
    {
        return IsLeft ? left(Left!) : right(Right!);
    }

}