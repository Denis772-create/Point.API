namespace Point.Domain.Entities.CompanyAggregate;

public class Shop : Entity
{
    public string? Phone { get; private set; }
    public TimeOnly? OpeningTime { get; private set; }
    public TimeOnly? ClosingTime { get; private set; }
    public Guid CompanyId { get; private set; }
    public ShopLocation? ShopLocation { get; private set; }

    public Shop(ShopLocation? shopLocation, Guid companyId,
        TimeOnly? closingTime = null, TimeOnly? openingTime = null,
        string? phone = null) : base(Guid.NewGuid())
    {
        ShopLocation = shopLocation;
        CompanyId = companyId;
        ClosingTime = closingTime;
        OpeningTime = openingTime;
        Phone = phone;
    }

    public Shop(Guid companyId,
        TimeOnly? closingTime = null, TimeOnly? openingTime = null,
        string? phone = null) : base(Guid.NewGuid())
    {
        CompanyId = companyId;
        ClosingTime = closingTime;
        OpeningTime = openingTime;
        Phone = phone;
    }

    public Shop(ShopLocation shopLocation) : base(Guid.NewGuid())
    {
        ShopLocation = shopLocation;
    }

    public void UpdateWorkingTime(TimeOnly? openingTime, TimeOnly? closingTime)
    {
        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }
}
