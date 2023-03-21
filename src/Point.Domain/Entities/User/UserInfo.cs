namespace Point.Domain.Entities.User;

public class UserInfo
{
    private static IPrincipalProvider? _principalProvider;

    public Guid Id { get; }
    public string NameIdentifier { get; }
    public string? GivenName { get; }
    public string? Surname { get; }
    public string? PrincipalName { get; }

    private UserInfo(Guid id, string nameIdentifier, string? givenName, string? surname, string? principalName)
    {
        Id = id;
        NameIdentifier = nameIdentifier;
        GivenName = givenName;
        Surname = surname;
        PrincipalName = principalName;
    }

    public static void SetProvider(IPrincipalProvider provider)
    {
        _principalProvider = provider;
    }

    public static UserInfo? Current()
    {
        if (_principalProvider == null)
            throw new InvalidOperationException($"{nameof(_principalProvider)} is not set.");

        var principal = _principalProvider.GetCurrent();
        if (principal?.Identity is not { IsAuthenticated: true }) return null;

        var claims = principal.Claims.ToList();

        var oidClaim = claims.SingleOrDefault(x => x.Type.Equals(@"objectidentifier"));
        var nameIdClaim = claims.Single(x => x.Type.Equals(ClaimTypes.NameIdentifier));
        var givenNameClaim = claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.GivenName));
        var surnameClaim = claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.Surname));
        var upnClaim = claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.Upn));

        var id = Guid.Parse(oidClaim is not null ? oidClaim.Value : nameIdClaim.Value);
        return new UserInfo(id, nameIdClaim.Value, givenNameClaim?.Value, surnameClaim?.Value, upnClaim?.Value);
    }
}