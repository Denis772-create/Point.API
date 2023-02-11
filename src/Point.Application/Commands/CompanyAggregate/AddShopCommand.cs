namespace Point.Application.Commands.CompanyAggregate;

public class AddShopCommand : IRequest<Guid>
{
    public Guid CompanyId { get; set; }
    public ShopDto Shop { get; set; }

    public class AddShopCommandValidator : AbstractValidator<ShopDto>
    {
        public AddShopCommandValidator()
        {
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
        }
    }
}

public class AddShopCommandHandler : IRequestHandler<AddShopCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly GeometryFactory _geometryFactory;

    public AddShopCommandHandler(ICompanyRepository companyRepository,
        GeometryFactory geometryFactory)
    {
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        _geometryFactory = geometryFactory ?? throw new ArgumentNullException(nameof(geometryFactory));
    }

    public async Task<Guid> Handle(AddShopCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetAsync(request.CompanyId);
        var newShop = request.Shop;
        if (company != null)
        {
            var shopLocation = ShopDto.MapToShopLocation(newShop, _geometryFactory);
            var isExists = company.Shops.Any(s => Equals(s.ShopLocation, shopLocation));

            if (!isExists)
            {
                company.AddShop(shopLocation, newShop.OpeningTime,
                    newShop.ClosingTime, newShop.Phone);

                _companyRepository.Update(company);
                await _companyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return company.Shops.FirstOrDefault(s => s.ShopLocation == shopLocation).Id;
            }
            return Guid.Empty;
        }
        throw new KeyNotFoundException();
    }
}