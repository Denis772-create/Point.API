namespace Point.Domain.Exceptions;

public class CardDomainException : DomainException
{
    public CardDomainException()
    {
    }

    public CardDomainException(string message)
        : base(message)
    {
    }

    public CardDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}