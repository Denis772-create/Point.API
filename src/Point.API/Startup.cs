namespace Point.API;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var apiConfiguration = Configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>();
        services.AddSingleton(apiConfiguration ?? throw new ArgumentNullException(nameof(ApiConfiguration)));

        services.AddHttpContextAccessor();
        services.AddScoped<IPrincipalProvider, HttpContextPrincipalProvider>();

        services.AddOptions()
                .AddApiConfiguration()
                .AddCustomHealthCheck(Configuration)
                .ConfigureApiVersions()
                .ConfigureSwagger(apiConfiguration)
                .AddApplicationLayer(Configuration)
                .AddInfrastructureLayer(Configuration);
    }

    public void Configure(IApplicationBuilder app, ApiConfiguration apiConfig)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        app.UseSwagger()
           .UseSwaggerUI(opt =>
           {
               opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Point.Shop API v1");
               opt.OAuthClientId(apiConfig.OidcSwaggerUIClientId);
               opt.OAuthAppName(apiConfig.ApiName);
               opt.OAuthUsePkce();
           });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}