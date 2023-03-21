namespace Point.Infrastructure.Authentication;

public class HttpContextPrincipalProvider : IPrincipalProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextPrincipalProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public ClaimsPrincipal? GetCurrent()
    {
        return _httpContextAccessor.HttpContext?.User;
    }
}