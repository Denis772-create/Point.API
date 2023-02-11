namespace Point.Infrastructure.EF;

public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    public DbSet<Company> Companies { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Discount> Discounts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
        => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShopConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new ClientRequestConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);
        var result = await SaveChangesAsync(cancellationToken);
        if (result > 0)
            return true;
        return false;
    }
}