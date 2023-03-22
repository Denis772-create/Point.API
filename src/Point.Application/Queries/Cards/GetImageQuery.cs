namespace Point.Application.Queries.Cards;

public class GetImageQuery : IRequest<OperationResult<ImageDto>>
{
    public Guid Id { get; }

    public GetImageQuery(Guid id)
    {
        Id = id;
    }
}

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, OperationResult<ImageDto>>
{
    private readonly IPhotoService _photoService;

    public GetImageQueryHandler(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    public async Task<OperationResult<ImageDto>> Handle(GetImageQuery query, CancellationToken ct)
    {
        var url = await _photoService.GetPhotoUrlByIdAsync(query.Id, ct);

        if (url is not null)
            return OperationResult<ImageDto>.Success(new ImageDto(url));

        return OperationResult<ImageDto>
            .Failure(ValidationErrors.DoesNotExist($"Photo with ID {query.Id}"));
    }
}