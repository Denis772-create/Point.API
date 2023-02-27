namespace Point.Application.Queries.Cards;

public class GetCardTemplateQuery : IRequest<OperationResult<CardTemplateDto>>
{
    public GetCardTemplateQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public class GetCardTemplateQueryHandler : IRequestHandler<GetCardTemplateQuery, OperationResult<CardTemplateDto>>
{
    public Task<OperationResult<CardTemplateDto>> Handle(GetCardTemplateQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}