namespace Point.Application.Commands.Companies;

public class AddCompanyCommand : TransactionalCommand<OperationResult<Guid>>

{
    public string Name { get; set; }
    public string? Instagram { get; set; }
    public string? Telegram { get; set; }
    public string SupportNumber { get; set; }
    public Guid OwnerId { get; set; }

    public class CreateCompanyCommandValidator
        : AbstractValidator<AddCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.OwnerId).NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}

public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, OperationResult<Guid>>
{
    private readonly IRepository<Company> _repository;

    public AddCompanyCommandHandler(IRepository<Company> repository)
        => _repository = repository;

    public Task<OperationResult<Guid>> Handle(AddCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var newCompany = new Company(request.Name,
            request.SupportNumber,
            request.Instagram,
            request.Telegram);

        _repository.Add(newCompany);

        return Task.FromResult(OperationResult<Guid>.Success(newCompany.Id));
    }
}