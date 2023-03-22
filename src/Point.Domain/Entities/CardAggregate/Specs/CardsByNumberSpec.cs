namespace Point.Domain.Entities.CardAggregate.Specs;

public sealed class ByNumberSpec : Specification<Card>  
{
    public ByNumberSpec(string cardNumber)
    {
        Query
            .Include(c => c.CardTemplate)
            .ThenInclude(ct => ct.BackgroundImage)
            .Where(c => c.CardNumber == cardNumber);
    }
}