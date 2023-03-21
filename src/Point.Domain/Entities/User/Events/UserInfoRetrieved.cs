namespace Point.Domain.Entities.User.Events;

public record UserInfoRetrieved(
    DateTimeOffset Timestamp,
    Guid UserId,
    string NameIdentifier,
    string? GivenName,
    string? Surname,
    string? PrincipalName) : Event(Guid.NewGuid(), Timestamp);