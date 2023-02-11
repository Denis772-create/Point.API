namespace Point.Domain.Entities.CompanyAggregate;

public class Company : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Instagram { get; private set; }
    public string Telegram { get; private set; }
    public string SupportNumber { get; private set; }   
    public Guid OwnerId { get; private set; }

    private readonly List<Shop> _shops;
    public IReadOnlyCollection<Shop> Shops => _shops;
    private readonly List<Discount> _discounts;
    public IReadOnlyCollection<Discount> Discounts => _discounts;


    public Company(string name, Guid ownerId, string supportNumber,
        string instagram = null, string telegram = null)
    {
        Name = name;
        Instagram = instagram;
        Telegram = telegram;
        OwnerId = ownerId;
        SupportNumber = supportNumber;
        _shops = new();
        _discounts = new();
    }

    public void AddShop(ShopLocation location,
        TimeOnly? openingTime = null, TimeOnly? closingTime = null, string phone = null)
    {
        var existingShop =
            _shops.FirstOrDefault(o => Equals(o.ShopLocation, location));

        if (existingShop != null) return;
        var shop = new Shop(location, Id, closingTime, openingTime, phone);
        _shops.Add(shop);
    }

    public void AddDiscount(string name, string description,
        DateTime startDate, DateTime expirationDate)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.NullOrEmpty(description, nameof(description));
        Guard.Against.AgainstExpression(startDate => startDate >= DateTime.Now,
            startDate, nameof(startDate));
        Guard.Against.AgainstExpression(expirationDate =>expirationDate >= DateTime.Now, 
            expirationDate, nameof(expirationDate));

        _discounts.Add(new Discount(expirationDate, startDate, description, name));
    }

    public void UpdateWorkingTimeInShop(Guid shopId,
        TimeOnly openingTime, TimeOnly closingTime)
    {
        var shop = _shops.FirstOrDefault(s => s.Id == shopId);
        if (shop != null)
            shop.UpdateWorkingTime(openingTime, closingTime);
    }
}