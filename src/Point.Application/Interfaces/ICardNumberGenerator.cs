namespace Point.Application.Interfaces;

public interface ICardNumberGenerator
{
    string GenerateCardNumber(Guid userId);
}