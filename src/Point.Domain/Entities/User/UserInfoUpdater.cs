namespace Point.Domain.Entities.User;

public class UserInfoUpdater
{
    private readonly IMediator _mediator;

    public UserInfoUpdater(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PropagateCurrentUserInfo(CancellationToken ct)
    {
        var currentUserInfo = UserInfo.Current();

        if (currentUserInfo is null) throw new InvalidOperationException("User is not authenticated.");

        var e = new UserInfoRetrieved(
            DateTimeOffset.Now,
            currentUserInfo.Id,
            currentUserInfo.NameIdentifier,
            currentUserInfo.GivenName,
            currentUserInfo.Surname,
            currentUserInfo.PrincipalName);

       await _mediator.Publish(e, ct);
    }
}