namespace Point.Application.Interfaces;

public interface ICardService
{
    Task<Guid> CreateAsync(CreateCardDto template, CancellationToken ct);
    Task<bool> NewBonusesAsync(Guid id, int count, CancellationToken ct);

    Task<CardDto> GetByIdAsync(Guid id, CancellationToken ct);

    Task<int> DeleteAsync(Guid id, CancellationToken ct);

    Task<List<CardDto>> ListByUserIdAsync(Guid id, ApiPageFilter filter, CancellationToken ct);

    Task<CardDto> GetByCardNumberAsync(string number, CancellationToken ct);
}