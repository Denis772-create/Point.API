namespace Point.Application.Queries;

public class GetCompanyQuery : IRequest<CompanyWithShopsDto>
{
    public Guid CompanyId { get; set; }

    public GetCompanyQuery(Guid companyId)
        => CompanyId = companyId;

    public class GetCompanyQueryValidator
        : AbstractValidator<GetCompanyQuery>
    {
        public GetCompanyQueryValidator()
           => RuleFor(x => x.CompanyId).NotEmpty();
    }
}

public class GetCompanyQueryHandler
    : IRequestHandler<GetCompanyQuery, CompanyWithShopsDto>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
    }

    public async Task<CompanyWithShopsDto> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetAsync(request.CompanyId);
        if (company != null)
            return CompanyWithShopsDto.MapFromCompany(company);

        throw new KeyNotFoundException($"Company not fount by key - {request.CompanyId}");
    }
}