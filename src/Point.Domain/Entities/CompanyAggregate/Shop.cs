namespace Point.Domain.Entities.CompanyAggregate;

public class Shop : Entity
{
    public string Phone { get; private set; }
    public TimeOnly? OpeningTime { get; private set; }
    public TimeOnly? ClosingTime { get; private set; }
    public Guid CompanyId { get; private set; }
    public ShopLocation ShopLocation { get; private set; }

    public Shop(ShopLocation location, Guid companyId,
        TimeOnly? closingTime = null, TimeOnly? openingTime = null,
        string phone = null)
    {
        ShopLocation = location;
        CompanyId = companyId;
        ClosingTime = closingTime;
        OpeningTime = openingTime;
        Phone = phone;
    }

    protected Shop() { }

    public Shop(ShopLocation shopLocation)
    {
        ShopLocation = shopLocation;
    }

    public void UpdateWorkingTime(TimeOnly openingTime, TimeOnly closingTime)
    {
        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }
}
