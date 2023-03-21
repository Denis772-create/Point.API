namespace Point.Domain.SeedWork;

public abstract class Entity
{
	public virtual Guid Id { get; protected set; }

	private readonly List<INotification> _domainEvents = new();
	public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

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
		_domainEvents.Add(eventItem);
	}

	public void RemoveDomainEvent(INotification eventItem)
		=> _domainEvents?.Remove(eventItem);

	public void ClearDomainEvents() => _domainEvents?.Clear();

	public override bool Equals(object? obj)
	{
		if (obj is not Entity item)
			return false;

		if (ReferenceEquals(this, item))
			return true;

		if (GetType() != item.GetType())
			return false;

		if (item.IsTransient() || IsTransient())
			return false;

		return item.Id == Id;
	}

	public override int GetHashCode() => base.GetHashCode();

	public static bool operator ==(Entity? left, Entity? right) => left?.Equals(right) ?? Equals(right, null);
	public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}