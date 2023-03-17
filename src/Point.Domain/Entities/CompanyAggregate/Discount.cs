namespace Point.Domain.Entities.CompanyAggregate;

public class Discount : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset? StartDate { get; private set; }
    public DateTimeOffset? ExpirationDate { get; private set; }

    public Discount(DateTimeOffset? expirationDate, DateTimeOffset? startDate,
        string description, string name) : base(Guid.NewGuid())
    {
        ExpirationDate = expirationDate;
        StartDate = startDate;
        Description = description;
        Name = name;
    }

    public void UpdateDiscountInfo(string name, string description,
        DateTimeOffset startDate, DateTimeOffset expirationDate)
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