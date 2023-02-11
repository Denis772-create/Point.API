namespace Point.Application.Queries;

public class NearestShopQuery : IRequest<ShopLocationDto>
{
    public Guid CompanyId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public class NearestShopQueryValidator
        : AbstractValidator<NearestShopQuery>
    {
        public NearestShopQueryValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.Latitude).Must(x => x >= -90 && x <= 90);
            RuleFor(x => x.Longitude).Must(x => x >= -180 && x <= 180);
        }
    }
}

public class NearestShopQueryHandler : IRequestHandler<NearestShopQuery, ShopLocationDto>
{
    private readonly ICompanyRepository _companyRepository;

    public NearestShopQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public Task<ShopLocationDto> Handle(NearestShopQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}