namespace Point.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        => services.AddOptions()
                   .AddControllers(opt => opt.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                   .AddJsonOptions(opt
                       =>
                   {
                       opt.JsonSerializerOptions.WriteIndented = true;
                       opt.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
                   })
                   .Services;

    public static IServiceCollection ConfigureApiVersions(this IServiceCollection services)
        => services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        });

    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        => services.AddHealthChecks()
                   .AddCheck("self", () => HealthCheckResult.Healthy())
                   .AddCheck("ShopDb-check",
                       new SqlConnectionHealthCheck(configuration["ConnectionStrings:AppDbConnection"]),
                           HealthStatus.Unhealthy,
                           new[] { "shop-db" })
                   .Services;

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, ApiConfiguration apiConfiguration)
      => services.AddSwaggerGen(options =>
      {
          options.SwaggerDoc("v1", new OpenApiInfo
          {
              Title = "Point API",
              Version = "v1"
          });

          options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
          {
              Type = SecuritySchemeType.OAuth2,
              Flows = new OpenApiOAuthFlows
              {
                  AuthorizationCode = new OpenApiOAuthFlow
                  {
                      AuthorizationUrl = new Uri($"{apiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                      TokenUrl = new Uri($"{apiConfiguration.IdentityServerBaseUrl}/connect/token"),
                      Scopes = new Dictionary<string, string>
                      {
                          { apiConfiguration.OidcApiName, apiConfiguration.ApiName }
                      }

                  }
              }
          });
          options.OperationFilter<AuthorizeCheckOperationFilter>();
      });

    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

            var retry = Policy.Handle<SqlException>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(8),
                });

            retry.Execute(() =>
            {
                context.Database.Migrate();
                seeder(context);
            });

            logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while migrating the database used on context {nameof(TContext)}");
        }

        return host;
    }

    public static IServiceCollection AddApplicationLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICardNumberGenerator, CardNumberGenerator>();
        services.AddScoped<IQrCodeGenerator, QrCodeGenerator>();

        services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        services.AddValidatorsFromAssemblyContaining(typeof(AddCompanyCommand));
        return services.AddMediatorModule();
    }

    public static IServiceCollection AddMediatorModule(this IServiceCollection services)
    {
        services.AddMediatR(typeof(AddCompanyCommand).Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PrincipalSetterBehavior<,>));
        return services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserInfoUpdaterBehavior<,>));
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration);
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<ITransactionContext, TransactionContext>();
        return services.AddScoped<IRequestManager, RequestManager>();
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(configuration["ConnectionStrings:AppDbConnection"], options =>
            {
                options.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), null);
                options.UseNetTopologySuite();
            });
        });
        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection service, IConfiguration configuration)
    {
        var apiConfig = configuration.GetSection(nameof(ApiConfiguration))
            .Get<ApiConfiguration>() ?? throw new ArgumentNullException(nameof(ApiConfiguration), "ApiConfiguration cannot be null.");

        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = apiConfig.IdentityServerBaseUrl;
                options.RequireHttpsMetadata = apiConfig.RequireHttpsMetadata;
                options.Audience = apiConfig.OidcApiName;
            });

        return service;
    }
}
