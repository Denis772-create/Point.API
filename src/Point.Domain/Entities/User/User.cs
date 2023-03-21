namespace Point.Domain.Entities.User;

public class User : Entity, IAggregateRoot
{
    public User(Guid id) : base(id)
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