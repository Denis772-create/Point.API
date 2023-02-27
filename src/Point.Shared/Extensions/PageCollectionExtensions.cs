namespace Point.Shared.Extensions;

public static class PageCollectionExtensions
{
    public static async Task<IPage<T>> TakePageAsync<T>(this IOrderedQueryable<T> source, IPageFilter filter,
        CancellationToken cancellationToken)
    {
        var offset = GetOffset(filter.Offset);
        var count = GetCount(filter.Count);

        var total = await source.CountAsync(cancellationToken);
        var items = await source.Skip(offset)
            .Take(count)
            .ToArrayAsync(cancellationToken);
        return new Page<T>(total, items);
    }

    public static IPage<T> TakePage<T>(this IOrderedEnumerable<T> source, IPageFilter filter)
    {
        var offset = GetOffset(filter.Offset);
        var count = GetCount(filter.Count);

        var enumerated = source.ToArray();

        var total = enumerated.Length;
        var items = enumerated.Skip(offset)
            .Take(count)
            .ToArray();

        return new Page<T>(total, items);
    }

    public static ISpecificationBuilder<T> TakePage<T>(this IOrderedSpecificationBuilder<T> source, IPageFilter filter)
    {
        var offset = GetOffset(filter.Offset);
        var count = GetCount(filter.Count);

        return source.Skip(offset).Take(count);
    }

    private static int GetOffset(int? offsetRaw)
    {
        var offset = 0;
        if (offsetRaw > 0) offset = offsetRaw.Value;

        return offset;
    }

    private static int GetCount(int? countRaw)
    {
        const int countMax = Paging.CountMax;
        const int countDefault = Paging.CountDefault;
        var count = countDefault;
        if (countRaw is > 0 and <= countMax) count = countRaw.Value;

        return count;
    }
}
