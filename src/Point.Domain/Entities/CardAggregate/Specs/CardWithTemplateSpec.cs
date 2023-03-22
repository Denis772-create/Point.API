namespace Point.Domain.Entities.CardAggregate.Specs;

public sealed class CardWithTemplateSpec : Specification<Card>, ISingleResultSpecification
{
    public CardWithTemplateSpec(Guid id)
    {
        Query
            .AsNoTracking()
            .Include(x => x.CardTemplate)
            .Where(x => x.Id == id);
    }
}