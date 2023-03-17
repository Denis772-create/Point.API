namespace Point.Application.Commands.Cards;

public class CreateCardCommand : TransactionalCommand<OperationResult<Guid>>
{
    public CreateCardCommand(CreateCardDto input)
    {
        Input = input;
    }

    public CreateCardDto Input { get; }
}

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, OperationResult<Guid>>
{
    private readonly CardFactory _cardFactory;

    public CreateCardCommandHandler(CardFactory cardFactory)
    {
        _cardFactory = cardFactory;
    }

    public Task<OperationResult<Guid>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var result = _cardFactory.TryCreate(request.Input);

        return Task.FromResult(result.Map(x => x.Id));
    }
}