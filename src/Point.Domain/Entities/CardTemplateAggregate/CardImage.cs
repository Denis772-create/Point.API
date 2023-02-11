namespace Point.Domain.Entities.CardTemplateAggregate;

public class CardImage : Entity
{
    public string Name { get; set; }
    public bool IsCommon { get; set; }

    public CardImage(bool isCommon, string name)
    {
        IsCommon = isCommon;
        Name = name;
    }

    public CardImage() { }
}