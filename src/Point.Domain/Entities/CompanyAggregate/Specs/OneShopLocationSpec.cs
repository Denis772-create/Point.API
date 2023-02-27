namespace Point.Domain.Entities.CompanyAggregate.Specs;

public sealed class OneShopLocationSpec : Specification<Company>, ISingleResultSpecification
{
    public OneShopLocationSpec(Guid shopId)
    {
        Query
            .Include(c => c.Shops)
            .Where(x => 
                x.Shops.Select(s => s.Id).Contains(shopId));
    }
}