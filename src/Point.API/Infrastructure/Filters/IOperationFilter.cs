namespace Point.API.Infrastructure.Filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    private readonly ApiConfiguration _apiConfiguration;

    public AuthorizeCheckOperationFilter(ApiConfiguration apiConfiguration)
    {
        _apiConfiguration = apiConfiguration;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType != null &&
                           (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                            || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

        if (!hasAuthorize) return;

        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                [
                    new OpenApiSecurityScheme {Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"}
                    }
                ] = new[] { _apiConfiguration.OidcApiName }
            }
        };
    }
}