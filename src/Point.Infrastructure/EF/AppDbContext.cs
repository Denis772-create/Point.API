using Point.Domain.Entities.User;

namespace Point.Infrastructure.EF;

public class AppDbContext : DbContext, IUnitOfWork, IStoredProcedureRepository
{
    private readonly IMediator _mediator;

    public DbSet<Company> Companies { get; set; } = null!; 
    public DbSet<Shop> Shops { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;
    // TODO: add configuration
    public DbSet<Card> Cards { get; set; } = null!;
    public DbSet<CardImage> CardImages{ get; set; } = null!;
    public DbSet<CardTemplate> CardTemplates{ get; set; } = null!;
    public DbSet<EfEvent> Events { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
        => _mediator = mediator;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShopConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new ClientRequestConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CardConfiguration());
        modelBuilder.ApplyConfiguration(new CardTemplateConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<T>> ExecuteStoredProcedure<T>(string storedProcedureName,
        params SqlParameter[] parameters) where T : class
    {
        var sqlParams = parameters
            .Select(p => new SqlParameter($"@{p.ParameterName}", p.Value))
            .ToArray();

        return await Set<T>()
            .FromSqlRaw($"{storedProcedureName} {string.Join(", ", parameters
                .Select(p => $"@{p.ParameterName}"))}", sqlParams)
            .ToListAsync();
    }

    public async Task<int> ExecuteStoredProcedureNonQuery(string storedProcedureName, params SqlParameter[] parameters)
    {
        var sqlParams = parameters
            .Select(p => new SqlParameter($"@{p.ParameterName}", p.Value))
            .ToArray();

        return await Database.ExecuteSqlRawAsync($"{storedProcedureName} {string.Join(", ", parameters
            .Select(p => $"@{p.ParameterName}"))}", sqlParams);
    }
}