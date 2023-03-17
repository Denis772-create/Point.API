﻿namespace Point.Application.Behaviors;

public class TransactionBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly ITransactionContext _transactionContext;

    public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger,
        ITransactionContext transactionContext)
    {
        _logger = logger ??
                  throw new ArgumentException(nameof(ILogger<TransactionBehaviour<TRequest, TResponse>>));
        _transactionContext = transactionContext ??
                              throw new ArgumentException(nameof(ITransactionContext));
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        {
            if (request is not ITransactional) return await next();

            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (_transactionContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _transactionContext.DbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _transactionContext.BeginTransactionAsync();
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})",
                            transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}",
                            transaction.TransactionId, typeName);

                        await _transactionContext.CommitTransactionAsync(transaction);
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}