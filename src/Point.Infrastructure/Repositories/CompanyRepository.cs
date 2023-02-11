namespace Point.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _shopContext;
    public IUnitOfWork UnitOfWork => _shopContext;

    public CompanyRepository(AppDbContext shopContext)
        => _shopContext = shopContext;

    public void Add(Company company)
        => _shopContext.Companies.Add(company);

    public async Task<Company> GetAsync(Guid companyId)
    {
        var company = await _shopContext.Companies
            .FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null)
        {
            company = _shopContext
                .Companies
                .Local
                .FirstOrDefault();
        }
        if (company != null)
        {
            await _shopContext.Entry(company)
                .Collection(i => i.Shops)
                .LoadAsync();
        }
        return company;
    }

    public void Update(Company company)
        => _shopContext.Entry(company).State = EntityState.Modified;

    public async Task<List<Company>> GetAllAsync(bool withRelated = false)
    {
        var companies = _shopContext.Companies.AsNoTracking();

        if (withRelated)
        {
            return await companies
                .Include(i => i.Shops)
                .ToListAsync();
        }
        return await companies.ToListAsync();
    }

    public async Task<List<ShopLocation>> GetAllShopLocationsAsync(Guid companyId)
        => (await _shopContext.Companies
                .AsNoTracking()
                .Include(x => x.Shops)
                .ThenInclude(x => x.ShopLocation)
                .FirstOrDefaultAsync(i => i.Id == companyId))?
            .Shops
            .Select(x => x.ShopLocation)
            .ToList();

    public Task DeleteAsync(Guid companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<ShopLocation> GetShopLocationAsync(Guid shopId)
        => (await _shopContext.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync())?
            .ShopLocation;
}