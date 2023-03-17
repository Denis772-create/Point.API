using Fluent = FluentValidation.Results;

namespace Point.Application.Queries.Companies;

public class GetCompanyQuery : IRequest<OperationResult<CompanyWithShopsDto>>
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

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, OperationResult<CompanyWithShopsDto>>
{
    private readonly IRepository<Company> _repository;

    public GetCompanyQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<CompanyWithShopsDto>> Handle(GetCompanyQuery request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);
        if (company != null)
            return OperationResult<CompanyWithShopsDto>
                .Success(CompanyWithShopsDto.MapFromCompany(company));

        return OperationResult<CompanyWithShopsDto>
            .Failure(new Fluent.ValidationResult(new[]
            {
                new Fluent.ValidationFailure("", "Company doesn't exist.")
            }));
    }
}