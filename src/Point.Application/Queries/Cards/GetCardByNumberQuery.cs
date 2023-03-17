namespace Point.Application.Queries.Cards;
public class GetCardByNumberQuery : IRequest<OperationResult<CardDto>>
{
    public GetCardByNumberQuery(string number)
    {
        Number = number;
    }

    public string Number { get; }
}

public class GetCardByNumberQueryHandler : IRequestHandler<GetCardByNumberQuery, OperationResult<CardDto>>
{
    private readonly IRepository<Card> _repository;

    public GetCardByNumberQueryHandler(IRepository<Card> repository)
    {
        _repository = repository;
    }
    public async Task<OperationResult<CardDto>> Handle(GetCardByNumberQuery request, CancellationToken ct)
    {
        var card = await _repository.FirstOrDefaultAsync(new ByNumberSpec(request.Number), ct);

        return card is not null
            ? OperationResult<CardDto>.Success(CardDto.FromCard(card))
            : OperationResult<CardDto>.Failure(ValidationErrors.DoesNotExist("Card"));
    }
}