namespace Point.Application.Queries.Companies;

public class GetDiscountQuery : IRequest<OperationResult<DiscountDto>>
{
    public GetDiscountQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}