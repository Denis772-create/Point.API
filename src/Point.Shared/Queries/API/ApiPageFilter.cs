namespace Point.Shared.Queries.API;

public record ApiPageFilter : IPageFilter
{
    public int? Offset { get; set; }
    public int? Count { get; set; }
}
