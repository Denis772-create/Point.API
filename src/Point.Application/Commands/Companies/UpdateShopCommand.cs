namespace Point.Application.Commands.Companies;

public class UpdateShopCommand : TransactionalCommand<OperationResult<Guid>>
{
    public UpdateShopCommand(Guid companyId, Guid shopId, ShopDto input)
    {
        CompanyId = companyId;
        ShopId = shopId;
        Input = input;
    }

    public Guid CompanyId { get; set; }
    public Guid ShopId { get; set; }
    public ShopDto Input { get; set; }
}

public class UpdateShopCommandHandler : IRequestHandler<UpdateShopCommand, OperationResult<Guid>>
{
    private readonly IRepository<Company> _repository;
    private readonly GeometryFactory _geometryFactory;

    public UpdateShopCommandHandler(IRepository<Company> repository, GeometryFactory geometryFactory)
    {
        _repository = repository;
        _geometryFactory = geometryFactory;
    }

    public async Task<OperationResult<Guid>> Handle(UpdateShopCommand request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);

        if (company is null)
            return OperationResult<Guid>
                .Failure(ValidationErrors.DoesNotExist("Company"));

        company.UpdateShop(request.ShopId, request.Input.Phone, request.Input.OpeningTime,
             request.Input.ClosingTime, ShopDto.MapToShopLocation(request.Input, _geometryFactory));

        return OperationResult<Guid>.Success(request.ShopId);
    }
}