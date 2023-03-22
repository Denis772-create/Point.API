using FluentValidation.Results;

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
    private readonly IRepository<Card> _repository;

    public CreateCardCommandHandler(CardFactory cardFactory, IRepository<Card> repository)
    {
        _cardFactory = cardFactory;
        _repository = repository;
    }

    public Task<OperationResult<Guid>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var result = _cardFactory.TryCreate(request.Input);

        if (result.IsSuccessful)
        {
            _repository.Add(result.Value!);
            return Task.FromResult(result.Map(x => x.Id));
        }

        return Task.FromResult(OperationResult<Guid>
            .Failure(new FluentValidation.Results.ValidationResult(new[]
                {
                    new ValidationFailure(nameof(CreateCardCommand),"Invalid user or card template id")
                })));
    }
}