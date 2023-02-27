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
    [HttpGet("{compId:Guid}")]
    public async Task<IActionResult> GetOne(Guid compId, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetCompanyQuery(compId), ct);

        return result.Match<IActionResult>(Ok, NotFound);
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
}