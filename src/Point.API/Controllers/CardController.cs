namespace Point.API.Controllers;

public class CardController : BaseController
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCardDto cardDto, CancellationToken ct = default)
    {
        var newCardId = await _cardService.CreateAsync(cardDto, ct);
        if (newCardId == default) return BadRequest();

        return Created(newCardId);
    }

    [HttpPut("{cardId:guid}/bonuses")]
    public async Task<IActionResult> NewBonuses([FromRoute] Guid cardId, [FromBody] BonusesInput input,
        CancellationToken ct = default)
    {
        var newCardId = await _cardService.NewBonusesAsync(cardId, input.Count, ct);

        return !newCardId ? BadRequest() : Ok();
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> PageByUserId([FromRoute] Guid userId, ApiPageFilter filter,
        CancellationToken ct = default)
    {
        if (userId != default)
        {
            var responseList = await _cardService.ListByUserIdAsync(userId, filter, ct);
            return Ok(responseList);
        }
        return BadRequest();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ById([FromRoute] Guid id, CancellationToken ct = default)
    {
        if (id != default)
        {
            var response = await _cardService.GetByIdAsync(id, ct);
            return response != null
                ? Ok(response)
                : BadRequest();
        }
        return BadRequest();
    }

    [HttpGet("{cardNumber}")]
    public async Task<IActionResult> GetByCardNumber(string cardNumber, CancellationToken ct = default)
    {
        var response = await _cardService.GetByCardNumberAsync(cardNumber, ct);
        return response != null
            ? Ok(response)
            : BadRequest();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var result = await _cardService.DeleteAsync(id, ct);
        return result > 0 ? Ok() : BadRequest();
    }
}