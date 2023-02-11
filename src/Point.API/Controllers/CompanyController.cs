namespace Point.API.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
public class CompanyController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
    {
        var commandResult = await Mediator.Send(command);
        if (commandResult != Guid.Empty)
            return Created(commandResult);

        return BadRequest();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        // TODO: replace all try/catch
        try
        {
            return Ok(await Mediator.Send(new GetAllCompaniesQuery()));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("all/without-shops")]
    public async Task<IActionResult> GetAllWithoutShops()
    {
        try
        {
            return Ok(await Mediator.Send(new GetAllCompaniesWithoutShopsQuery()));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("{compId:Guid}")]
    public async Task<IActionResult> Get(Guid compId)
    {
        try
        {
            return Ok(await Mediator.Send(new GetCompanyQuery(compId)));
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost("add-shop")]
    public async Task<IActionResult> AddShop([FromBody] AddShopCommand command)
    {
        var commandResult = await Mediator.Send(command);
        if (commandResult != Guid.Empty)
            return Created(commandResult);

        return BadRequest();
    }
}