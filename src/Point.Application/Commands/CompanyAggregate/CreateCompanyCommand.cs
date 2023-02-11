namespace Point.Application.Commands.CompanyAggregate;

public class CreateCompanyCommand : IRequest<Guid>

{
    public string Name { get; set; }
    public string Instagram { get; set; }
    public string Telegram { get; set; }
    public string SupportNumber { get; set; }
    public Guid OwnerId { get; set; }

    public class CreateCompanyCommandValidator
        : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.OwnerId).NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}

public class CreateDeliveryCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;

    public CreateDeliveryCommandHandler(ICompanyRepository companyRepository)
        => _companyRepository = companyRepository
                                ?? throw new ArgumentNullException(nameof(companyRepository));

    public async Task<Guid> Handle(CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var newCompany = new Company(request.Name,
            request.OwnerId,
            request.SupportNumber,
            request.Instagram,
            request.Telegram);

        _companyRepository.Add(newCompany);

        await _companyRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return newCompany.Id;
    }
}