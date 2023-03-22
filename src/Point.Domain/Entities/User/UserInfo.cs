using IdentityModel;

namespace Point.Domain.Entities.User;

public class UserInfo
{
    private static IPrincipalProvider? _principalProvider;

    public Guid Id { get; }
    public string NameIdentifier { get; }
    public string? GivenName { get; }
    public string? Surname { get; }
    public string? PrincipalName { get; }
    public string Email { get; }

    private UserInfo(Guid id, string nameIdentifier, string email, string? givenName, string? surname, string? principalName)
    {
        Id = id;
        NameIdentifier = nameIdentifier;
        GivenName = givenName;
        Surname = surname;
        PrincipalName = principalName;
        Email = email;
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

        var oidClaim = claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
        var nameIdClaim = claims.Single(x => x.Type.Equals(ClaimTypes.NameIdentifier));
        var givenNameClaim = claims.SingleOrDefault(x => x.Type.Equals(JwtClaimTypes.GivenName));
        var surnameClaim = claims.SingleOrDefault(x => x.Type.Equals(JwtClaimTypes.FamilyName));
        var upnClaim = claims.SingleOrDefault(x => x.Type.Equals(JwtClaimTypes.Name));
        var email = claims.Single(x => x.Type.Equals(ClaimTypes.Email));

        var id = Guid.Parse(oidClaim is not null ? oidClaim.Value : nameIdClaim.Value);
        return new UserInfo(id, nameIdClaim.Value, email.Value, 
            givenNameClaim?.Value, surnameClaim?.Value, upnClaim?.Value);
    }
}