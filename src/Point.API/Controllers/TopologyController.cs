namespace Point.API.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
public class TopologyController : BaseController
{
    [HttpGet("all-shops/{CompanyId:Guid}")]
    public async Task<IActionResult> AllShopLocationsByCompanyId([FromRoute] AllShopLocationsQuery query,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(query, ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpGet("one-shop/{shopId:Guid}")]
    public async Task<IActionResult> ShopLocationById([FromRoute] ShopLocationQuery query,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(query, ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpGet("nearest-shop")]
    public async Task<IActionResult> GetNearestShop([FromBody] NearestShopQuery query,
        CancellationToken ct = default)
    {

        var result = await Mediator.Send(query, ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }

    [HttpGet("specific-area")]
    public async Task<IActionResult> GetNearestShopsInSpecificArea([FromBody] NearestShopsInSpecificAreaQuery query,
        CancellationToken ct = default)
    {
        var result = await Mediator.Send(query, ct);

        return result.Match<IActionResult>(Ok, NotFound);
    }
}