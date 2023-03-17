using Fluent = FluentValidation.Results;

namespace Point.Application.Queries.Companies;

public class GetAllCompaniesWithoutShopsQuery : PageFilter, IRequest<OperationResult<List<CompanyDto>>>
{

}

public class GetAllCompaniesWithoutShopsQueryHandler
    : IRequestHandler<GetAllCompaniesWithoutShopsQuery, OperationResult<List<CompanyDto>>>
{
    private readonly IRepository<Company> _repository;

    public GetAllCompaniesWithoutShopsQueryHandler(IRepository<Company> repository)
        => _repository = repository;

    public async Task<OperationResult<List<CompanyDto>>> Handle(GetAllCompaniesWithoutShopsQuery request,
        CancellationToken ct)
    {
        var companies = await _repository.ListAsync(new CompaniesWithoutShops(request), ct);

        if (companies.Any())
        {
            var companiesPage = companies
                .Select(x => x.ToDto()).ToList();

            return OperationResult<List<CompanyDto>>.Success(companiesPage);
        }

        return OperationResult<List<CompanyDto>>
            .Failure(new Fluent.ValidationResult(new[]
            {
                new Fluent.ValidationFailure("", "Companies not found")
            }));
    }
}