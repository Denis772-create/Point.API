namespace Point.Domain.SeedWork;

public interface IRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    IUnitOfWork UnitOfWork { get; }
}