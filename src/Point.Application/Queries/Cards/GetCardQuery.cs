namespace Point.Application.Queries.Cards;

public class GetCardQuery : IRequest<OperationResult<CardDto>>
{
    public GetCardQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public class GetCardQueryHandler : IRequestHandler<GetCardQuery, OperationResult<CardDto>>
{
    private readonly IRepository<Card> _repository;

    public GetCardQueryHandler(IRepository<Card> repository)
    {
        _repository = repository;
    }
    public async Task<OperationResult<CardDto>> Handle(GetCardQuery query, CancellationToken ct)
    {
        var card = await _repository
            .FirstOrDefaultAsync(new CardWithTemplateSpec(query.Id), ct);

        return card is not null 
            ? OperationResult<CardDto>.Success(CardDto.FromCard(card)) 
            : OperationResult<CardDto>.Failure(ValidationErrors.DoesNotExist("Card"));
    }
}