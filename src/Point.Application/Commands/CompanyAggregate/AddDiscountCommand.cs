namespace Point.Application.Commands.CompanyAggregate;

public class AddDiscountCommand : IRequest<Guid>
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

public class AddDiscountCommandHandler : IRequestHandler<AddDiscountCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;

    public AddDiscountCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
    }

    public async Task<Guid> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetAsync(request.CompanyId);
        if (company != null)
        {
            company.AddDiscount(request.Name, request.Description,
                request.StartDate, request.ExpirationDate);

            _companyRepository.Update(company);
            await _companyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return company.Discounts.LastOrDefault().Id;
        }
        return Guid.Empty;
    }
}
