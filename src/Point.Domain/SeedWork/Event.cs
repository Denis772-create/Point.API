namespace Point.Domain.SeedWork;

public record Event(Guid Id, DateTimeOffset Timestamp) : IEvent;