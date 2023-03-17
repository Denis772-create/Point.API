namespace Point.Application.Commands.Cards;

public class AddNewBonusesCommand : TransactionalCommand<OperationResult<Guid>>
{
    public AddNewBonusesCommand(BonusesInput input, Guid cardId)
    {
        Input = input;
        CardId = cardId;
    }

    public BonusesInput Input { get; }
    public Guid CardId { get; }
}

public class AddNewBonusesCommandHandler : IRequestHandler<AddNewBonusesCommand, OperationResult<Guid>>
{
    private readonly IRepository<Card> _repository;

    public AddNewBonusesCommandHandler(IRepository<Card> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Guid>> Handle(AddNewBonusesCommand request, CancellationToken ct)
    {
        var card = await _repository.GetByIdAsync(request.CardId, ct);

        if (card is null)
            return OperationResult<Guid>
                .Failure(ValidationErrors.DoesNotExist("Card"));

        var result = card.IncreaseSteps(request.Input.Count);

        if (result.IsSuccessful) _repository.Update(card);

        return result.Map(_ => card.Id);
    }
}