namespace Point.Shared.Queries;

public record Page<T>(int Total, T[] Items) : IPage<T>;
