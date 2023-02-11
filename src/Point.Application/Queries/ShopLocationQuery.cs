namespace Point.Application.Queries;

public class ShopLocationQuery : IRequest<ShopLocationDto>
{
    public Guid ShopId { get; set; }

    public class ShopLocationQueryValidator
        : AbstractValidator<ShopLocationQuery>
    {
        public ShopLocationQueryValidator()
            => RuleFor(x => x.ShopId).NotEmpty();
    }
}

public class ShopLocationQueryHandler
    : IRequestHandler<ShopLocationQuery, ShopLocationDto>
{
    private readonly ICompanyRepository _companyRepository;

    public ShopLocationQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<ShopLocationDto> Handle(ShopLocationQuery request,
        CancellationToken cancellationToken)
    {
        var shoplocation = await _companyRepository
            .GetShopLocationAsync(request.ShopId);

        if (shoplocation != null)
            return ShopLocationDto.MapToShopLocationDto(shoplocation);

        throw new ShopDomainException("Companies not found");
    }
}