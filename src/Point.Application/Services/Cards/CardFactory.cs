namespace Point.Application.Services.Cards;

public class CardFactory
{
    private readonly ICardNumberGenerator _cardNumberGenerator;
    private readonly IQrCodeGenerator _qRCodeGenerator;

    public CardFactory(ICardNumberGenerator cardNumberGenerator,
        IQrCodeGenerator qRCodeGenerator)
    {
        _cardNumberGenerator = cardNumberGenerator;
        _qRCodeGenerator = qRCodeGenerator;
    }

    public OperationResult<Card>TryCreate(CreateCardDto cardDto)
    {
        // TODO: business validation 

        var card = new Card(cardDto.TemplateId, cardDto.UserId);

        var cardNumber = _cardNumberGenerator.GenerateCardNumber(cardDto.UserId);
        card.AddCardNumber(cardNumber);

        var qrCode = _qRCodeGenerator.CreateQrCode(cardNumber);
        card.UpdateQrCode(new QrCode(qrCode));

        return OperationResult<Card>.Success(card);
    }
}