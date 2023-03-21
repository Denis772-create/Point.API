namespace Point.Domain.Interfaces;

public interface IPrincipalProvider
{
    ClaimsPrincipal? GetCurrent();
}