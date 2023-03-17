namespace Point.Domain.Entities;

public class CardImage : Entity, IAggregateRoot
{
    public bool IsShared { get; set; }

    public CardImage(bool isShared) : base(Guid.NewGuid())
    {
        IsShared = isShared;
    }

    public CardImage(Guid id, bool isShared) : base(id)
    {
        IsShared = isShared;
    }
}