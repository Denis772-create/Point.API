namespace Point.Domain.Entities.CompanyAggregate.Specs;

public sealed class CompaniesWithoutShops : Specification<Company>
{
    public CompaniesWithoutShops(IPageFilter filter)
    {
        Query
            .Include(x => x.Shops)
            .OrderBy(x => x.Name)
            .TakePage(filter);
    }
}