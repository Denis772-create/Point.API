namespace Point.API.Controllers;

public class CardController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCardDto cardDto, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new CreateCardCommand(cardDto), ct);

        return await result.MatchAsync(
            id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [HttpPut("{cardId:guid}")]
    public async Task<IActionResult> Update([FromQuery] Guid cardId, [FromBody] CardDto cardDto, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new UpdateCardCommand(cardDto, cardId), ct);

        return await result.MatchAsync(
            id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [HttpPost("{cardId:guid}/bonuses")]
    public async Task<IActionResult> NewBonuses([FromRoute] Guid cardId, [FromBody] BonusesInput input,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(new AddNewBonusesCommand(input, cardId), ct);

        return await result.MatchAsync(
            id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [ProducesResponseType(typeof(IPage<CardDto>), 200)]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> PageByUserId([FromRoute] Guid userId, ApiPageFilter filter,
        CancellationToken ct = default)
    {
        // TODO: move to AutoMapper
        var result = await Mediator.Send(new GetCardsQuery(userId)
        {
            Count = filter.Count,
            Offset = filter.Offset
        }, ct);

        return result.ToResponse(Ok, NotFound);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOne([FromRoute] Guid id, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetCardQuery(id), ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpGet("card-number/{cardNumber}")]
    public async Task<IActionResult> GetByCardNumber(string cardNumber, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetCardByNumberQuery(cardNumber), ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new DeleteCardCommand(id), ct);

        return result.Match<IActionResult>(_ => Ok(), BadRequest);
    }
}