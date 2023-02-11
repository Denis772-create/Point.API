namespace Point.Application.Services;

public class CardNumberGenerator : ICardNumberGenerator
{
    public string GenerateCardNumber(Guid userId)
    {
        return Guid.NewGuid().ToString().Substring(1, 7);
    }
}