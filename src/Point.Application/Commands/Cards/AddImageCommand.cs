namespace Point.Application.Commands.Cards;

public class AddImageCommand : TransactionalCommand<OperationResult<Guid>>
{
    public AddImageCommand(ImageInput input)
    {
        Input = input;
    }

    public ImageInput Input { get; }
}

public class AddImageCommandHandler : IRequestHandler<AddImageCommand, OperationResult<Guid>>
{
    private readonly IRepository<CardImage> _repository;
    private readonly IPhotoService _photoService;

    public AddImageCommandHandler(IRepository<CardImage> repository, IPhotoService photoService)
    {
        _repository = repository;
        _photoService = photoService;
    }

    public async Task<OperationResult<Guid>> Handle(AddImageCommand request, CancellationToken ct)
    {
        // TODO: validation
        var photoId = await _photoService.AddPhotoAsync(request.Input.Photo, ct);

        var newImage = new CardImage(photoId, request.Input.IsShared);

        _repository.Add(newImage);
        return OperationResult<Guid>.Success(photoId);
    }
}