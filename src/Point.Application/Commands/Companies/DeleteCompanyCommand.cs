namespace Point.Application.Commands.Companies;

public class DeleteCompanyCommand : TransactionalCommand<OperationResult>
{
    public DeleteCompanyCommand(Guid companyId)
    {
        CompanyId = companyId;
    }

    public Guid CompanyId { get; set; }
}

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, OperationResult>
{
    private readonly IRepository<Company> _repository;

    public DeleteCompanyCommandHandler(IRepository<Company> repository)
        => _repository = repository;

    public async Task<OperationResult> Handle(DeleteCompanyCommand request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);
        if (company is null)
            return OperationResult
                .Failure(ValidationErrors.DoesNotExist("Company"));

        _repository.Delete(company);

        return OperationResult.Success();
    }
}