namespace Point.Application.Commands.Cards;

public class DeleteCardCommand : TransactionalCommand<OperationResult>
{
    public DeleteCardCommand(Guid id)
    {
        Id = id;
    }

    public  Guid Id { get;}
}

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, OperationResult>
{
    private readonly IRepository<Card> _repository;

    public DeleteCardCommandHandler(IRepository<Card> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(DeleteCardCommand request, CancellationToken ct)
    {
        var existingCard = await _repository.GetByIdAsync(request.Id, ct);
        if (existingCard != null)
        {
            _repository.Delete(existingCard);
            return OperationResult.Success();
        }
        return OperationResult.Failure(ValidationErrors.DoesNotExist("Card"));
    }
}