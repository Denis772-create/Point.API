namespace Point.API;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();
        Log.Logger = CreateSerilogLogger(configuration);
        
        try
        {
            var host = BuildHost(args);

            Log.Information("Applying migrations ({ApplicationName})...");
            host.MigrateDbContext<AppDbContext>(context =>
            {
                new AppContextSeed()
                    .SeedAsync(context)
                    .Wait();
            });

            Log.Information("Starting web host ({ApplicationName})...");
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationName})!");
        }
    }

    public static IHost BuildHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
         => new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
}