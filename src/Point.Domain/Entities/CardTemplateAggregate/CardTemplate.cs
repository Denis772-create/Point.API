namespace Point.Domain.Entities.CardTemplateAggregate;

public class CardTemplate : Entity, IAggregateRoot
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int MaxBonuses { get; private set; }
    public bool IsFreeFirstPoint { get; private set; }
    public Guid CompanyId { get; private set; }
    public CardImage? BackgroundImage { get;  set; }
    public Guid BackgroundImageId { get;  set; }
    public CardTemplate(string title,
        string description, 
        int maxBonuses,
        bool isFreeFirstPoint,
        Guid companyId, 
        Guid backgroundImageId = default) : base(Guid.NewGuid())
    {
        Title = title;
        Description = description;
        MaxBonuses = maxBonuses;
        IsFreeFirstPoint = isFreeFirstPoint;    
        CompanyId = companyId;  
        BackgroundImageId = backgroundImageId;  
    }

    public void UpdateInfo(string title,
        string description,
        int maxBonuses)
    {
        Title = title;
        Description = description;
        MaxBonuses = maxBonuses;
    }

    public void UpdateImage(bool isShared)
    {
        BackgroundImage = new CardImage(isShared);
    }
}