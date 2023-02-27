namespace Point.Domain.Entities.CardAggregate.Specs;

public sealed class PageByUserIdSpec : Specification<Card>
{
    public PageByUserIdSpec(Guid userId, IPageFilter filter)
    {
        Query
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(x => x.CountSteps)
            .TakePage(filter);
    }
}