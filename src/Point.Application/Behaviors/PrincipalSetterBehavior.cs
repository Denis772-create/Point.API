namespace Point.Application.Behaviors;

public class PrincipalSetterBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPrincipalProvider _principalProvider;

    public PrincipalSetterBehavior(IPrincipalProvider principalProvider)
    {
        _principalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        UserInfo.SetProvider(_principalProvider);

        return await next();
    }
}