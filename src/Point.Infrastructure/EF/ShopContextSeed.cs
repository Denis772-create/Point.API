namespace Point.Infrastructure.EF;

public class ShopContextSeed
{
    public async Task SeedAsync(AppDbContext context)
    {
        var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(
            3, _ => TimeSpan.FromSeconds(5),
            (exception, _, _, _) =>
            {
                Console.WriteLine(exception);
            });

        await policy.ExecuteAsync(async () =>
        {
            await using (context)
            {
                if (!context.Companies.Any())
                {
                    context.Companies.AddRange(GetCompanies());
                    await context.SaveChangesAsync();
                }

                if (!context.Discounts.Any())
                {
                    context.Discounts.AddRange(GetDiscounts());
                    await context.SaveChangesAsync();
                }

                if (!context.Discounts.Any())
                {
                    context.Shops.AddRange(GetShops());
                    await context.SaveChangesAsync();
                }

                await context.SaveEntitiesAsync();
            }
        });
    }

    private IEnumerable<Company> GetCompanies()
        => new List<Company>
        {
                new("Microsoft", Guid.NewGuid(), "+375292436302")
        };

    private IEnumerable<Discount> GetDiscounts()
        => new List<Discount>
        {
            new(DateTime.MaxValue,DateTime.MinValue, "Discount from Microsoft", "Azure with VS Professional")
        };

    private IEnumerable<Shop> GetShops()
        => new List<Shop>
        {
            new(new ShopLocation("2/24", 3, 4, "Aybeck", "Tashkent", "UZ", "1003200", NetTopologySuite.Geometries.Point.Empty))
        };
}

