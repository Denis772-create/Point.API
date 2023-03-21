namespace Point.Domain.Entities;

public class EfEvent : Entity, IAggregateRoot
{
    public EfEvent() : base(Guid.NewGuid()) { }

    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Helps to filter events of a specific entity.
    /// </summary>
    public string EntityId { get; set; } = "";

    /// <summary>
    /// Identifies a type responsible for event serialization.
    /// </summary>
    public string Discriminator { get; set; } = "";

    public string SerializedValue { get; set; } = "";
}