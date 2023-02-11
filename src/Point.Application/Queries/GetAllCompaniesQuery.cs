namespace Point.Application.Queries;

public class GetAllCompaniesQuery : IRequest<List<CompanyWithShopsDto>>
{ }

public class GetAllCompaniesQueryHandler
    : IRequestHandler<GetAllCompaniesQuery, List<CompanyWithShopsDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository)
        => _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));

    public async Task<List<CompanyWithShopsDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync(true);

        if (companies.Any())
            return companies
                .Select(x => CompanyWithShopsDto.MapFromCompany(x))
                .ToList();

        throw new ShopDomainException("Companies not found");
    }
}