namespace Point.Application.EventHandlers;

public class UserEventHandler
    : INotificationHandler<UserInfoRetrieved>
{
    private readonly IRepository<User> _repository;

    public UserEventHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UserInfoRetrieved notification, CancellationToken cancellationToken)
    {
        var e = notification;
        var user = await _repository.GetByIdAsync(e.UserId, cancellationToken);

        if (user is null)
        {
            user = new User(e.UserId);

            _repository.Add(user);
        }

        user.NameIdentifier = e.NameIdentifier;
        user.UserPrincipalName = e.PrincipalName;
        user.Email = e.Email;
        user.DisplayName = BuildDisplayName(e.GivenName, e.Surname);
        user.LastUpdated = DateTime.UtcNow;

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        Console.WriteLine($"User '{user.Id}' is updated.");
    }

    private static string? BuildDisplayName(string? givenName, string? surname)
    {
        var parts = new[]
            {
                surname,
                givenName
            }.Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        return parts.Any() ? string.Join(", ", parts) : null;
    }
}