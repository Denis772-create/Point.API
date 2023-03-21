namespace Point.Domain.Interfaces;
public interface IEvent : INotification
{
    Guid Id { get; }
    DateTimeOffset Timestamp { get; }
}
