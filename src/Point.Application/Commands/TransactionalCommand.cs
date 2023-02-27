namespace Point.Application.Commands;

/// <summary>
/// Indicator for commands that are executed within a DB transaction.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class TransactionalCommand<TResult> : IRequest<TResult> ,ITransactional
{

}