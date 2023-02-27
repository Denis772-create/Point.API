namespace Point.Application.Commands;

public class IdentifiedCommand<TRequest, TResponse>
    : AbstractValidator<IdentifiedCommand<TRequest, TResponse>>,
        IRequest<TResponse> where TRequest : IRequest<TResponse>
{
    public IdentifiedCommand(TRequest command, Guid id)
    {
        Command = command;
        Id = id;

        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Command).NotNull();
    }

    public TRequest Command { get; }
    public Guid Id { get; }
}

// TODO: think about the need for this piece of logic
public class IdentifiedCommandHandler<TRequest, TResponse>
    : IRequestHandler<IdentifiedCommand<TRequest, TResponse>, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<IdentifiedCommandHandler<TRequest, TResponse>> _logger;
    private readonly IMediator _mediator;
    private readonly IRequestManager _requestManager;

    public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<TRequest, TResponse>> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(IdentifiedCommand<TRequest, TResponse> request,
        CancellationToken cancellationToken)
    {
        var alreadyExists = await _requestManager.ExistAsync(request.Id);
        if (alreadyExists) return CreateResultForDuplicateRequest();

        await _requestManager.CreateRequestForCommandAsync<TRequest>(request.Id);
        try
        {
            var command = request.Command;
            var commandName = command.GetGenericTypeName();
            var idProperty = string.Empty;
            var commandId = Guid.Empty;

            switch (command)
            {
            }

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                commandName,
                idProperty,
                commandId,
                command);

            var result = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation(
                "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                result,
                commandName,
                idProperty,
                commandId,
                command);

            return result;
        }
        catch (Exception)
        {
            return default;
        }
    }

    protected virtual TResponse CreateResultForDuplicateRequest() => default;
}