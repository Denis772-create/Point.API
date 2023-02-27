namespace Point.Application.Queries;

public class ShopLocationQuery : IRequest<OperationResult<ShopLocationDto>>
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
    : IRequestHandler<ShopLocationQuery, OperationResult<ShopLocationDto>>
{
    private readonly IRepository<Company> _repository;

    public ShopLocationQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<ShopLocationDto>> Handle(ShopLocationQuery request, CancellationToken ct)
    {
        var shopLocation = await _repository
            .FirstOrDefaultAsync(new OneShopLocationSpec(request.ShopId), ct);

        if (shopLocation != null)
        {
            var shopLocationDto = ShopLocationDto
                .MapToShopLocationDto(shopLocation.Shops
                    .First(x => x.Id == request.ShopId).ShopLocation);

            return OperationResult<ShopLocationDto>.Success(shopLocationDto);
        }

        return OperationResult<ShopLocationDto>.Failure(ValidationErrors.DoesNotExist("Companies"));
    }
}