namespace Point.Domain.Entities.CompanyAggregate;

public interface ICompanyRepository : IRepository<Company>
{
    void Add(Company company);

    void Update(Company company);

    Task<Company> GetAsync(Guid companyId);

    Task<List<Company>> GetAllAsync(bool withRelated = false);

    Task<List<ShopLocation>> GetAllShopLocationsAsync(Guid companyId);

    Task<ShopLocation> GetShopLocationAsync(Guid shopId);

    Task DeleteAsync(Guid companyId);
}