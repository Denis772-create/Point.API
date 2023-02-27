namespace Point.Application.Queries;

public class AllShopLocationsQuery : TransactionalCommand<OperationResult<IPage<ShopLocationDto>>>, IPageFilter
{
    public Guid CompanyId { get; set; }
    public int? Offset { get; set; }
    public int? Count { get; set; }

    public AllShopLocationsQuery(Guid companyId)
    {
        CompanyId = companyId;
    }

    public class AllShopLocationsQueryValidator
        : AbstractValidator<AllShopLocationsQuery>
    {
        public AllShopLocationsQueryValidator()
            => RuleFor(x => x.CompanyId).NotEmpty();
    }
}

public class AllShopLocationsQueryHandler
    : IRequestHandler<AllShopLocationsQuery, OperationResult<IPage<ShopLocationDto>>>
{
    private readonly IRepository<Company> _repository;

    public AllShopLocationsQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<IPage<ShopLocationDto>>> Handle(AllShopLocationsQuery request,
        CancellationToken ct)
    {
        var company = await _repository.FirstOrDefaultAsync(new ShopLocationsSpec(request.CompanyId), ct);

        if (company != null)
        {
            var locations = company.Shops
                .Select(s => ShopLocationDto.MapToShopLocationDto(s.ShopLocation))
                .OrderBy(l => l.Street)
                .TakePage(request);

            return OperationResult<IPage<ShopLocationDto>>.Success(locations);
        }
        return OperationResult<IPage<ShopLocationDto>>
            .Success(new Page<ShopLocationDto>(0, Array.Empty<ShopLocationDto>()));
    }
}