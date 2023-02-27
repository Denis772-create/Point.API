namespace Point.API.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]

public class DiscountController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(AddDiscountCommand command, CancellationToken ct = default)
    {
        var result = await Mediator.Send(command, ct);

        return await result.Match(id => GetOne(id, ct),
            validation => Task.FromResult<IActionResult>(BadRequest(validation)));
    }

    [HttpGet]
    public async Task<IActionResult> GetOne([FromQuery] Guid id, CancellationToken ct = default)
    {
        var result = await Mediator.Send(new GetDiscountQuery(id), ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }
}

