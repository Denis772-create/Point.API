namespace Point.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                      ?? throw new ArgumentNullException(nameof(_mediator));
}