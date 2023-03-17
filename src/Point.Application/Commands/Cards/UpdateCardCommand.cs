namespace Point.Application.Commands.Cards;

public class UpdateCardCommand : TransactionalCommand<OperationResult<Guid>>
{
    public UpdateCardCommand(CardDto input, Guid cardId)
    {
        Input = input;
        CardId = cardId;
    }

    public Guid CardId { get; }
    public CardDto Input { get; }
}

public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand, OperationResult<Guid>>
{
    private readonly IRepository<Card> _repository;

    public UpdateCardCommandHandler(IRepository<Card> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Guid>> Handle(UpdateCardCommand request, CancellationToken ct)
    {
        var card = await _repository.GetByIdAsync(request.CardId, ct);

        if (card is null)
            return OperationResult<Guid>
                .Failure(ValidationErrors.DoesNotExist("Card"));

        var template = request.Input.Template;
        return card.Update(new CardTemplate(template.Title, template.Description, template.MaxBonuses,
               template.IsFreeFirstPoint, template.CompanyId, template.BackgroundImage.Id));
    }
}