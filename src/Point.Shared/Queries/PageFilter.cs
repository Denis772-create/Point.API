namespace Point.Shared.Queries;

public class PageFilter : IPageFilter
{
    public int? Offset { get; set; }
    public int? Count { get; set; }
}