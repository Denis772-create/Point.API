using Point.Domain.SeedWork;

namespace Point.Application.Queries.Cards;

public class GetCardsQuery : PageFilter, IRequest<OperationResult<IPage<CardDto>>>
{
    public GetCardsQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}

public class GetCardsQueryHandler : IRequestHandler<GetCardsQuery, OperationResult<IPage<CardDto>>>
{
    private readonly IRepository<Card> _repository;

    public GetCardsQueryHandler(IRepository<Card> repository)
    {
        _repository = repository;
    }
    public async Task<OperationResult<IPage<CardDto>>> Handle(GetCardsQuery request, CancellationToken ct)
    {
        var cards = await _repository.ListAsync(new PageByUserIdSpec(request.UserId, request), ct);

        // TODO: do we need to use Total ?
        return cards.Any()
            ? OperationResult<IPage<CardDto>>
                .Success(new Page<CardDto>(0, cards.Select(CardDto.FromCard).ToArray()))
            : OperationResult<IPage<CardDto>>
                .Success(new Page<CardDto>(0, Array.Empty<CardDto>()));
    }
}