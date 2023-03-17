namespace Point.Domain.Entities.CardAggregate;

public class Card : Entity, IAggregateRoot
{
	public int CountSteps { get; private set; }
	public string CardNumber { get; private set; }
	public QrCode QrCode { get; private set; }
	public bool IsActive { get; private set; } = true;
	public Guid UserId { get; private set; }
	public Guid CardTemplateId { get; private set; }
	public CardTemplate? CardTemplate { get; set; }
	public Company? Company { get; set; }

	public Card(Guid cardTemplateId, Guid userId) : base(Guid.NewGuid())
	{
		CardTemplateId = cardTemplateId;
		UserId = userId;
	}

	public OperationResult IncreaseSteps(int countSteps)
	{
		if (CardTemplate?.MaxBonuses >= countSteps && countSteps > 0)
			return OperationResult.Failure(new ValidationResult(new[]
			{
				new ValidationFailure(nameof(countSteps), $"The number of steps must be between 0 and {CardTemplate.MaxBonuses}!")
			}));

		CountSteps += countSteps;

		return OperationResult.Success();
	}

	public void UpdateQrCode(QrCode newQr)
	{
		if (!string.IsNullOrEmpty(newQr.Code))
			QrCode = newQr;
	}

	public void AddCardNumber(string number)
	{
		CardNumber = number;
	}

    public OperationResult<Guid> Update(CardTemplate cardTemplate)
    {
        if (cardTemplate is null)
            return OperationResult<Guid>.Failure(new ValidationResult(new[]
            {
                new ValidationFailure(nameof(cardTemplate), "Card Template is required.")
            }));

		CardTemplate = cardTemplate;

        return OperationResult<Guid>.Success(Id);
    }

}