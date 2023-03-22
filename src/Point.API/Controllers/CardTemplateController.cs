namespace Point.API.Controllers;

public class CardTemplateController : BaseController
{
    [ProducesResponseType(typeof(CardTemplateDto), 200)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOne(Guid id, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetCardTemplateQuery(id), ct);

        return result.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CardTemplateDto cardTemplateDto, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new CreateCardTemplateCommand(cardTemplateDto), ct);

        return await result.MatchAsync(
            id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [ProducesResponseType(typeof(CardTemplateDto), 200)]
    [HttpGet("image/{id:guid}")]
    public async Task<IActionResult> GetOneImage(Guid id, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetImageQuery(id), ct);

        return result.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpPost("image")]
    public async Task<IActionResult> CreateImage([FromForm] ImageInput input, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new AddImageCommand(input), ct);

        return await result.MatchAsync(
            id => GetOneImage(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }
}