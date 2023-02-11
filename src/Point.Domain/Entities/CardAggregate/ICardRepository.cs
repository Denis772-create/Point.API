namespace Point.Domain.Entities.CardAggregate;

public interface ICardRepository
{
    Task<Guid> Create(Card card, CancellationToken ct);

    Task<int> Update(Card card, CancellationToken ct);

    Task<Card> ById(Guid id, CancellationToken ct);

    Task<List<Card>> ListByUserId(Guid id, ApiPageFilter filter, CancellationToken ct);

    Task<int> Delete(Guid id, CancellationToken ct);

    Task<Card> ByCardNumber(string number, CancellationToken ct);
}