namespace Point.API.Controllers;

public class CardTemplateController : BaseController
{
    private readonly ICardTemplateService _cardTemplateService;

    public CardTemplateController(ICardTemplateService cardTemplateService)
    {
        _cardTemplateService = cardTemplateService
                               ?? throw new ArgumentNullException(nameof(cardTemplateService));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CardTemplateDto cardTemplateDto, CancellationToken ct = default)
    {
        var creationResult = await _cardTemplateService.CreateAsync(cardTemplateDto, ct);
        return creationResult != default
            ? Created(creationResult)
            : BadRequest();
    }

    [HttpPost("shared-image")]
    public async Task<IActionResult> CreateImage([FromQuery] ImageInput input, CancellationToken ct = default)
    {
        var creationResult = await _cardTemplateService.CreateImageAsync(input.Name, ct);
        return creationResult != default
            ? Created(creationResult)
            : BadRequest();
    }
}