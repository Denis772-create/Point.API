namespace Point.API.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
public class CompaniesController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddCompanyCommand command,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(command, ct);

        return await result.Match(id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [ProducesResponseType(typeof(CompanyWithShopsDto), 200)]
    [HttpGet("{compId:guid}")]
    public async Task<IActionResult> GetOne(Guid compId, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetCompanyQuery(compId), ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpPut("{compId:guid}")]
    public async Task<IActionResult> Update([FromBody] CompanyDto input, [FromQuery] Guid compId,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(new UpdateCompanyCommand(input, compId), ct);

        return await result.Match(id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [HttpDelete("{compId:guid}")]
    public async Task<IActionResult> Delete([FromQuery] Guid compId, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new DeleteCompanyCommand(compId), ct);

        return result.Match<IActionResult>(_ => Ok(), BadRequest);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCompaniesWithoutShopsQuery query,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(query, ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [ProducesResponseType(200)]
    [HttpPost("shop")]
    public async Task<IActionResult> AddShop([FromBody] AddShopCommand command,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(command, ct);

        return await result.Match(id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [HttpGet("{compId:guid}/shops/{shopId:guid}")]
    public async Task<IActionResult> GetShop(Guid compId, Guid shopId, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetShopQuery(compId, shopId), ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpPut("{compId:guid}/shops/{shopId:guid}")]
    public async Task<IActionResult> UpdateShop([FromQuery] Guid compId, [FromQuery] Guid shopId, [FromBody] ShopDto input,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(new UpdateShopCommand(compId, shopId, input), ct);

        return await result.Match(id => GetShop(compId, shopId, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [HttpDelete("{compId:guid}/shops/{shopId:guid}")]
    public async Task<IActionResult> DeleteShop([FromQuery] Guid compId, [FromQuery] Guid shopId, [FromBody] ShopDto input,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(new DeleteShopCommand(compId, shopId), ct);

        return result.Match<IActionResult>(_ => Ok(), BadRequest);
    }
}