namespace Point.Application.Commands.Companies;

public class DeleteShopCommand : TransactionalCommand<OperationResult>
{
    public DeleteShopCommand(Guid companyId, Guid shopId)
    {
        CompanyId = companyId;
        ShopId = shopId;
    }

    public Guid CompanyId { get; set; }
    public Guid ShopId { get; set; }
}

public class DeleteShopCommandHandler : IRequestHandler<DeleteShopCommand, OperationResult>
{
    private readonly IRepository<Company> _repository;

    public DeleteShopCommandHandler(IRepository<Company> repository, GeometryFactory geometryFactory)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(DeleteShopCommand request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);

        return company is null 
            ? OperationResult.Failure(ValidationErrors.DoesNotExist("Company")) 
            : company.RemoveShop(request.ShopId);
    }

}