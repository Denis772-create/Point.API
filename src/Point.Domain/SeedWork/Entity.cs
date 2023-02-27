namespace Point.Domain.SeedWork;

public abstract class Entity
{
    public virtual Guid Id { get; protected set; }

    private List<INotification> _domainEvents;
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    public bool IsTransient() => Id == default;

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    protected Entity(Guid id)
    {
        Id = id;
        if (IsTransient())
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = CreatedAt;
        }
    }

    public virtual void SetDateUpdated()
    {
        if (!IsTransient()) UpdatedAt = DateTime.UtcNow;
    }

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
        => _domainEvents?.Remove(eventItem);

    public void ClearDomainEvents() => _domainEvents?.Clear();

    public override bool Equals(object? obj)
    {
        if (obj is null || !(obj is Entity item))
            return false;

        if (ReferenceEquals(this, item))
            return true;

        if (GetType() != item.GetType())
            return false;

        if (item.IsTransient() || IsTransient())
            return false;
        else
            return item.Id == Id;
    }

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (Equals(left, null))
            return (Equals(right, null)) ? true : false;
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}