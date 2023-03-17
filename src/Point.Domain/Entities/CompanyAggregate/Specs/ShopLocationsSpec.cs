namespace Point.Domain.Entities.CompanyAggregate.Specs;

public sealed class ShopLocationsSpec : Specification<Company>, ISingleResultSpecification
{
    public ShopLocationsSpec(Guid companyId)
    {
        Query
            .AsNoTracking()
            .Include(c => c.Shops)
            .ThenInclude(s => s.ShopLocation)
            .Where(x => x.Id == companyId);
    }
}