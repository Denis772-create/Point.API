namespace Point.Domain.Entities.CompanyAggregate;

public class Discount : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime ExpirationDate { get; private set; }

    public Discount(DateTime expirationDate, DateTime startDate,
        string description, string name)
    {
        ExpirationDate = expirationDate;
        StartDate = startDate;
        Description = description;
        Name = name;
    }

    protected Discount() { }

    public void UpdateDiscountInfo(string name, string description,
        DateTime startDate, DateTime expirationDate)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.NullOrEmpty(description, nameof(description));
        Guard.Against.Default(startDate, nameof(startDate));
        Guard.Against.Default(expirationDate, nameof(expirationDate));

        Name = name;
        Description = description;
        StartDate = startDate;
        ExpirationDate = expirationDate;
    }
}