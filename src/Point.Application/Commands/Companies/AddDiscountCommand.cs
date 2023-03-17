namespace Point.Application.Commands.Companies;

public class AddDiscountCommand : TransactionalCommand<OperationResult<Guid>>
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }

    public class AddDiscountCommandValidator : AbstractValidator<AddDiscountCommand>
    {
        public AddDiscountCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
        }
    }
}

public class AddDiscountCommandHandler : IRequestHandler<AddDiscountCommand, OperationResult<Guid>>
{
    private readonly IRepository<Company> _repository;

    public AddDiscountCommandHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Guid>> Handle(AddDiscountCommand request, CancellationToken ct)
    {
        var company = await _repository.GetByIdAsync(request.CompanyId, ct);

        if (company != null)
        {
            company.AddDiscount(request.Name, request.Description,
                request.StartDate, request.ExpirationDate);

            _repository.Update(company);
            await _repository.UnitOfWork.SaveEntitiesAsync(ct);

            return OperationResult<Guid>
                .Success(company.Discounts.LastOrDefault()!.Id);
        }

        return OperationResult<Guid>
            .Failure(ValidationErrors.DoesNotExist("Company"));
    }
}
