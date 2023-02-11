namespace Point.API.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]

public class DiscountController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(AddDiscountCommand command)
    {
        var commandResult = await Mediator.Send(command);
        if (commandResult != Guid.Empty)
            return Created(commandResult);

        return BadRequest();
    }
}

