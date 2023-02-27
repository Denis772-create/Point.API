namespace Point.Domain.Entities;

public class User : Entity, IAggregateRoot
{
    public User() : base(Guid.NewGuid())
    { }

    /// <summary>
    /// This value is used as 'sub' in all events.
    /// </summary>
    public string NameIdentifier { get; set; } = "";
    public string? DisplayName { get; set; }
    public string? UserPrincipalName { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}