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
    private readonly IRepository<CardTemplate> _repository;

    public GetCardTemplateQueryHandler(IRepository<CardTemplate> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<CardTemplateDto>> Handle(GetCardTemplateQuery query, CancellationToken ct)
    {
        var cardTemplate = await _repository.GetByIdAsync(query.Id, ct);

        if (cardTemplate is not null)
            // TODO: move to AutoMapper
            return OperationResult<CardTemplateDto>.Success(new CardTemplateDto
            {
                CompanyId = cardTemplate.CompanyId,
                Description = cardTemplate.Description,
                IsFreeFirstPoint = cardTemplate.IsFreeFirstPoint,
                MaxBonuses = cardTemplate.MaxBonuses,
                Title = cardTemplate.Title,
                BackgroundImage = new BackgroundImageDto
                {
                    Id = cardTemplate.BackgroundImageId
                }
            });

        return OperationResult<CardTemplateDto>
            .Failure(ValidationErrors.DoesNotExist($"Card Template"));
    }
}