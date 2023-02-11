namespace Point.Domain.Exceptions;

public class ShopDomainException : Exception
{
    public ShopDomainException()
    {
    }

    public ShopDomainException(string message)
        : base(message)
    {
    }

    public ShopDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}