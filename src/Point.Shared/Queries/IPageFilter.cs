namespace Point.Shared.Queries;

public interface IPageFilter
{
    public int? Offset { get; set; }
    public int? Count { get; set; }
}