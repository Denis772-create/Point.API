namespace Point.Shared.Queries;

public interface IPage<out T>
{
    int Total { get; }
    T[] Items { get; }
}