namespace Point.Application.Queries.Companies;

public class GetShopQuery : IRequest<OperationResult<ShopDto>>
{
    public GetShopQuery(Guid shopId, Guid companyId)
    {
        ShopId = shopId;
        CompanyId = companyId;
    }

    public Guid CompanyId { get; set; }
    public Guid ShopId { get; set; }
}

public class GetShopQueryHandler : IRequestHandler<GetShopQuery, OperationResult<ShopDto>>
{
    private readonly IRepository<Company> _repository;

    public GetShopQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<ShopDto>> Handle(GetShopQuery request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);
        var shop = company?.Shops.FirstOrDefault(x => x.Id == request.ShopId);

        if (company != null && shop != null)
            return OperationResult<ShopDto>.Success(shop.ToDto());

        return OperationResult<ShopDto>
            .Failure(ValidationErrors.DoesNotExist("Company or Shop"));
    }
}