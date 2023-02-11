namespace Point.API.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
public class TopologyController : BaseController
{
    [HttpGet("all-shops/{CompanyId:Guid}")]
    public async Task<IActionResult> AllShopLocationsByCompanyId(
        [FromRoute] AllShopLocationsQuery query)
    {
        try
        {
            return Ok(await Mediator.Send(query));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("one-shop/{shopId:Guid}")]
    public async Task<IActionResult> ShopLocationById(
        [FromRoute] ShopLocationQuery query)
    {
        try
        {
            return Ok(await Mediator.Send(query));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("nearest-shop")]
    public async Task<IActionResult> GetNearestShop(
        [FromBody] NearestShopQuery query)
    {
        try
        {
            return Ok(await Mediator.Send(query));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("specific-area/{companyId:Guid}")]
    public async Task<IActionResult> GetNearestShopsInSpecificArea(
        [FromBody] NearestShopsInSpecificAreaQuery query,
        [FromRoute] Guid companyId)
    {
        // TODO: implement it
        return Ok();
    }
}