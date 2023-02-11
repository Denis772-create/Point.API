namespace Point.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                      ?? throw new ArgumentNullException(nameof(_mediator));

    // TODO: replace it with FluentValidation/ivc
    [NonAction]
    public CreatedResult Created(object value)
    {
        var uri = new Uri($"{Request.Scheme}://" +
                          $"{Request.Host}/api/" +
                          $"{GetType().Name.Replace("Controller", "")}/" +
                          $"{value}");

        return Created(uri, value);
    }
}