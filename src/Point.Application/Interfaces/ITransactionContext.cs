namespace Point.Application.Interfaces;

public interface ITransactionContext
{
    DbContext DbContext { get; }
    bool HasActiveTransaction { get; }
    IDbContextTransaction GetCurrentTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    void RollbackTransaction();
}

