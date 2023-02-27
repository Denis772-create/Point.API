namespace Point.Infrastructure.EF;

public class TransactionContext : ITransactionContext
{
    private readonly AppDbContext _appDbContext;
    private IDbContextTransaction _currentTransaction;

    public TransactionContext(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public DbContext DbContext => _appDbContext;
    public bool HasActiveTransaction => _currentTransaction != null;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction =
            await _appDbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await _appDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}