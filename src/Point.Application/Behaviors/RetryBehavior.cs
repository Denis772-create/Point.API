namespace Point.Application.Behaviors;

public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly AsyncRetryPolicy<TResponse> _retryPolicy;

    public RetryBehavior()
    {
        _retryPolicy = Policy<TResponse>.Handle<Exception>()
            .RetryAsync(3);
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await _retryPolicy.ExecuteAsync(
            async () => await next());
    }
}