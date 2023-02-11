namespace Point.Application.Services;

public class CardService : ICardService
{
    private readonly ICardNumberGenerator _cardNumberGenerator;
    private readonly IQrCodeGenerator _qRCodeGenerator;
    private readonly ICardRepository _cardRepository;

    public CardService(ICardNumberGenerator cardNumberGenerator,
        IQrCodeGenerator qRCodeGenerator,
        ICardRepository cardRepository)
    {
        _cardNumberGenerator = cardNumberGenerator;
        _qRCodeGenerator = qRCodeGenerator;
        _cardRepository = cardRepository;
    }

    public async Task<Guid> CreateAsync(CreateCardDto cardDto, CancellationToken ct)
    {
        var card = new Card(cardDto.TemplateId, cardDto.UserId);

        var cardNumber = _cardNumberGenerator.GenerateCardNumber(cardDto.UserId);
        card.AddCardNumber(cardNumber);

        var qrCode = _qRCodeGenerator.CreateQrCode("inputData"); // TODO: QR generation
        card.UpdateQrCode(qrCode);

        return await _cardRepository.Create(card, ct);
    }

    public async Task<bool> NewBonusesAsync(Guid id, int count, CancellationToken ct)
    {
        var card = await _cardRepository.ById(id, ct);
        if (card != null)
        {
            card.IncreaseSteps(count);
            await _cardRepository.Update(card, ct);
            
            return true;
        }

        return false;
    }

    public Task<int> DeleteAsync(Guid id, CancellationToken ct)
        => _cardRepository.Delete(id, ct);

    public async Task<List<CardDto>> ListByUserIdAsync(Guid id, ApiPageFilter filter, CancellationToken ct)
    {
        var cards = await _cardRepository.ListByUserId(id, filter, ct);

        if (cards.Any())
        {
            var cardsResponse = new List<CardDto>();
            cards.ForEach(card => cardsResponse.Add(CardDto.FromCard(card)));
            return cardsResponse;
        }
        return new();
    }

    public async Task<CardDto> GetByCardNumberAsync(string number, CancellationToken ct)
    {
        var card = await _cardRepository.ByCardNumber(number, ct);
        return card != null ? CardDto.FromCard(card) : null;
    }

    public async Task<CardDto> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var card = await _cardRepository.ById(id, ct);
        return card != null ? CardDto.FromCard(card) : null;
    }
}