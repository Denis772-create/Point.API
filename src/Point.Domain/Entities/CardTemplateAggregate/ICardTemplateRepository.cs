namespace Point.Domain.Entities.CardTemplateAggregate;

public interface ICardTemplateRepository
{
    Task<Guid> Create(CardTemplate card, CancellationToken ct);
    Task<Guid> CreateBackgroundImage(CardImage image, CancellationToken ct);
    Task Update(CardTemplate card, CancellationToken ct);
    Task<Card> ById(Guid id, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}