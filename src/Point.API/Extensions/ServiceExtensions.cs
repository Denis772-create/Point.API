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

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
      => services.AddSwaggerGen(s =>
      {
          s.SwaggerDoc("v1", new OpenApiInfo
          {
              Title = "Point API",
              Version = "v1",
              Description = "Service Shop for Point proj",
          });

          s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
          {
              In = ParameterLocation.Header,
              Description = "Place to add JWT with Bearer",
              Name = "Authorization",
              Type = SecuritySchemeType.ApiKey,
              Scheme = "Bearer"
          });

          s.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Name = "Bearer",
                            },
                            new List<string>()
                        }
               });
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
            logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
        }

        return host;
    }

    public static IServiceCollection AddApplicationLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        services.AddValidatorsFromAssemblyContaining(typeof(CreateCompanyCommand));
        return services.AddMediatorModule();
    }

    public static IServiceCollection AddMediatorModule(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateCompanyCommand).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddScoped<ICardNumberGenerator, CardNumberGenerator>();
        services.AddScoped<ICardTemplateService, CardTemplateService>();
        services.AddScoped<IQrCodeGenerator, QrCodeGenerator>();
        return services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration);
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ICardTemplateRepository, CardTemplateRepository>();
        services.AddScoped<ITransactionContext, ShopTransactionContext>();
        return services.AddScoped<IRequestManager, RequestManager>();
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseMySql(configuration["ConnectionStrings:AppDbConnection"],
                new MySqlServerVersion(new Version(8, 0, 30)), sqlOpt =>
                {
                    sqlOpt.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                    sqlOpt.EnableRetryOnFailure(15, TimeSpan.FromSeconds(20), null);
                    sqlOpt.UseNetTopologySuite();
                });
        });
        return services;
    }
}
