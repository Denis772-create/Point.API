namespace Point.Application.Queries;

public class AllShopLocationsQuery : IRequest<List<ShopLocationDto>>
{
    public Guid CompanyId { get; set; }

    public class AllShopLocationsQueryValidator
        : AbstractValidator<AllShopLocationsQuery>
    {
        public AllShopLocationsQueryValidator()
            => RuleFor(x => x.CompanyId).NotEmpty();
    }
}

public class AllShopLocationsQueryHandler
    : IRequestHandler<AllShopLocationsQuery, List<ShopLocationDto>>
{
    private readonly ICompanyRepository _companyRepository;

    public AllShopLocationsQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<List<ShopLocationDto>> Handle(AllShopLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var shoplocations = await _companyRepository
            .GetAllShopLocationsAsync(request.CompanyId);

        if (shoplocations.Any())
            return shoplocations
                .Select(x => ShopLocationDto.MapToShopLocationDto(x))
                .ToList();

        throw new ShopDomainException("Companies not found");
    }
}