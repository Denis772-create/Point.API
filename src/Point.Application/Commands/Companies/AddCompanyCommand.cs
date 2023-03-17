namespace Point.Application.Commands.Companies;

public class AddCompanyCommand : TransactionalCommand<OperationResult<Guid>>

{
    public AddCompanyCommand(CompanyDto input, Guid ownerId)
    {
        Input = input;
        OwnerId = ownerId;
    }

    public CompanyDto Input { get; set; }
    public Guid OwnerId { get; set; }

    public class CreateCompanyCommandValidator
        : AbstractValidator<AddCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.OwnerId).NotEqual(Guid.Empty);
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
        var newCompany = new Company(request.Input.Name,
            request.Input.SupportNumber,
            request.Input.Instagram,
            request.Input.Telegram);

        _repository.Add(newCompany);

        return Task.FromResult(OperationResult<Guid>.Success(newCompany.Id));
    }
}