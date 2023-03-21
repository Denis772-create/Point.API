namespace Point.Application.Behaviors;
public class UserInfoUpdaterBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly UserInfoUpdater _userInfoUpdater;

    public UserInfoUpdaterBehavior(UserInfoUpdater userInfoUpdater)
    {
        _userInfoUpdater = userInfoUpdater ?? throw new ArgumentNullException(nameof(userInfoUpdater));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isCommand = request.GetType()
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITransactional));

        if (isCommand) await _userInfoUpdater.PropagateCurrentUserInfo(cancellationToken);

        return await next.Invoke();
    }
}