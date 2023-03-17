namespace Point.Application.Commands.Cards;

public class CreateCardTemplateCommand : TransactionalCommand<OperationResult<Guid>>
{
    public CreateCardTemplateCommand(CardTemplateDto input)
    {
        Input = input;
    }

    public CardTemplateDto Input { get; }
}

public class CreateCardTemplateCommandHandler : IRequestHandler<CreateCardTemplateCommand, OperationResult<Guid>>
{
    private readonly IRepository<CardTemplate> _repository;

    public CreateCardTemplateCommandHandler(ICardTemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Guid>> Handle(CreateCardTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: validation
        var cardTemplate = new CardTemplate(request.Input.Title,
                request.Input.Description,
                request.Input.MaxBonuses,
                request.Input.IsFreeFirstPoint,
                request.Input.CompanyId,
                request.Input.BackgroundImage.Id);

        _repository.Add(cardTemplate);

        return OperationResult<Guid>.Success(cardTemplate.Id);
    }
}