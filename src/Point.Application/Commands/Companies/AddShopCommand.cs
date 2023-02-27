using FluentValidation.Results;
using Fluent = FluentValidation.Results;

namespace Point.Application.Commands.Companies;

public class AddShopCommand : TransactionalCommand<OperationResult<Guid>>
{
    public Guid CompanyId { get; set; }
    public ShopDto Shop { get; set; }

    public AddShopCommand(Guid companyId, ShopDto shopDto)
    {
        CompanyId = companyId;
        Shop = shopDto;
    }

    public class AddShopCommandValidator : AbstractValidator<ShopDto>
    {
        public AddShopCommandValidator()
        {
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
        }
    }
}

public class AddShopCommandHandler : IRequestHandler<AddShopCommand, OperationResult<Guid>>
{
    private readonly IRepository<Company> _repository;
    private readonly GeometryFactory _geometryFactory;

    public AddShopCommandHandler(IRepository<Company> companyRepository,
        GeometryFactory geometryFactory)
    {
        _repository = companyRepository;
        _geometryFactory = geometryFactory;
    }

    public async Task<OperationResult<Guid>> Handle(AddShopCommand request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);
        var newShop = request.Shop;

        if (company != null)
        {
            var shopLocation = ShopDto.MapToShopLocation(newShop, _geometryFactory);
            var isExists = company.Shops.Any(s => Equals(s.ShopLocation, shopLocation));

            if (!isExists)
            {
                company.AddShop(shopLocation, newShop.OpeningTime,
                    newShop.ClosingTime, newShop.Phone);

                _repository.Update(company);
                await _repository.UnitOfWork.SaveEntitiesAsync(ct);

                return OperationResult<Guid>.Success(
                    company.Shops.FirstOrDefault(s => Equals(s.ShopLocation, shopLocation))!.Id);
            }

            return OperationResult<Guid>.Failure(new Fluent.ValidationResult(new[]
            {
                new ValidationFailure("", "The store already exists for the specified company.")
            }));
        }

        return OperationResult<Guid>
            .Failure(ValidationErrors.DoesNotExist("Company"));
    }
}