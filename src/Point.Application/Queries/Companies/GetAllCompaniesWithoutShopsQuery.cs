using Fluent = FluentValidation.Results;

namespace Point.Application.Queries.Companies;

public class GetAllCompaniesWithoutShopsQuery : PageFilter, IRequest<OperationResult<List<CompanyWithoutShopsDto>>>
{

}

public class GetAllCompaniesWithoutShopsQueryHandler
    : IRequestHandler<GetAllCompaniesWithoutShopsQuery, OperationResult<List<CompanyWithoutShopsDto>>>
{
    private readonly IRepository<Company> _repository;

    public GetAllCompaniesWithoutShopsQueryHandler(IRepository<Company> repository)
        => _repository = repository;

    public async Task<OperationResult<List<CompanyWithoutShopsDto>>> Handle(GetAllCompaniesWithoutShopsQuery request,
        CancellationToken ct)
    {
        var companies = await _repository.ListAsync(new CompaniesWithoutShops(request), ct);

        if (companies.Any())
        {
            var companiesPage = companies
                .Select(CompanyWithoutShopsDto.MapFromCompany).ToList();

            return OperationResult<List<CompanyWithoutShopsDto>>.Success(companiesPage);
        }

        return OperationResult<List<CompanyWithoutShopsDto>>
            .Failure(new Fluent.ValidationResult(new[]
            {
                new Fluent.ValidationFailure("", "Companies not found")
            }));
    }
}