namespace Point.Domain.Entities.CardAggregate;

public class Card : Entity, IAggregateRoot
{
    public int CountSteps { get; private set; }
    public string CardNumber { get; private set; }
    public string QRCode { get; private set; }
    public bool IsActive { get; private set; } = true;
    public Guid UserId { get; private set; }
    public Guid CardTemplateId { get; private set; }
    public CardTemplate CardTemplate { get; set; }

    public Card() { }

    public Card(Guid cardTemplateId, Guid userId)
    {
        CardTemplateId = cardTemplateId;
        UserId = userId;
    }

    public void IncreaseSteps(int countSteps)
    {
        if (CardTemplate.MaxBonuses >= countSteps && countSteps <= 0)
            throw new CardDomainException(nameof(IncreaseSteps));

        CountSteps += countSteps;
    }

    public void UpdateQrCode(string newQr)
    {
        if (!string.IsNullOrEmpty(newQr))
            QRCode = newQr;
    }

    public void AddCardNumber(string number)
    {
        CardNumber = number;
    }
}