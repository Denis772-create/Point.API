using Point.Domain.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Point.Domain.Entities.CompanyAggregate;

public class Company : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Instagram { get; private set; }
    public string? Telegram { get; private set; }
    public string SupportNumber { get; private set; }

    private readonly List<Shop> _shops;
    public IReadOnlyCollection<Shop> Shops => _shops;

    private readonly List<Discount> _discounts;
    public IReadOnlyCollection<Discount> Discounts => _discounts;

    public Company(string name, string supportNumber,
        string? instagram = null, string? telegram = null) : base(Guid.NewGuid())
    {
        Name = name;
        Instagram = instagram;
        Telegram = telegram;
        SupportNumber = supportNumber;
        _shops = new List<Shop>();
        _discounts = new List<Discount>();
    }

    public void AddShop(ShopLocation location,
        TimeOnly? openingTime = null, TimeOnly? closingTime = null, string? phone = null)
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
        Guard.Against.AgainstExpression(startDate => startDate >= DateTime.Now, startDate, nameof(startDate));
        Guard.Against.AgainstExpression(expirationDate => expirationDate >= DateTime.Now, expirationDate, nameof(expirationDate));

        _discounts.Add(new Discount(expirationDate, startDate, description, name));
    }

    public void UpdateWorkingTime(Guid shopId,
        TimeOnly openingTime, TimeOnly closingTime)
    {
        var shop = _shops.FirstOrDefault(s => s.Id == shopId);
        if (shop != null)
            shop.UpdateWorkingTime(openingTime, closingTime);
    }

    public void Update(string name, string? instagram, string? telegram, string supportNumber)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.NullOrEmpty(supportNumber, nameof(supportNumber));

        Name = name;
        Instagram = instagram;
        Telegram = telegram;
        SupportNumber = supportNumber;
    }

    public OperationResult<Guid> UpdateShop(Guid shopId, string phone, 
        TimeOnly? openingTime, TimeOnly? closingTime, ShopLocation? shopLocation)
    {
        var shop = Shops.FirstOrDefault(x => x.Id == shopId);

        if (shop is null)
            return OperationResult<Guid>
                .Failure(ValidationErrors.DoesNotExist("Shop"));

        shop.Update(phone, shopLocation, openingTime, closingTime);

        return OperationResult<Guid>.Success(shopId);
    }

    public OperationResult RemoveShop(Guid shopId)
    {
        if (_shops.Any(x => x.Id == shopId))
            return OperationResult.Failure(ValidationErrors.DoesNotExist("Shop"));

        _shops.RemoveAll(x => x.Id == shopId);

        return OperationResult.Success();
    }
}