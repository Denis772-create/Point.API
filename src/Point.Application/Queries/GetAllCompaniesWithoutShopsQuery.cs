namespace Point.Application.Queries;

public class GetAllCompaniesWithoutShopsQuery : IRequest<List<CompanyWithoutShopsDto>>
{ }

public class GetAllCompaniesWithoutShopsQueryHandler
    : IRequestHandler<GetAllCompaniesWithoutShopsQuery, List<CompanyWithoutShopsDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesWithoutShopsQueryHandler(ICompanyRepository companyRepository)
        => _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));

    public async Task<List<CompanyWithoutShopsDto>> Handle(GetAllCompaniesWithoutShopsQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync();

        if (companies.Any())
            return companies
                .Select(x => CompanyWithoutShopsDto.MapFromCompany(x))
                .ToList();

        throw new ShopDomainException("Companies not found");
    }
}