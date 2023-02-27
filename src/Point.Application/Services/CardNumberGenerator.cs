namespace Point.Application.Services;

public class CardNumberGenerator : ICardNumberGenerator
{
    public string GenerateCardNumber(Guid userId)
    {
        // TODO: implement
        return Guid.NewGuid().ToString().Substring(1, 7);
    }
}