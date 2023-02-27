namespace Point.Infrastructure.EF.EntityConfigurations;

public class ShopConfiguration
    : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.ToTable("Shops");

        builder.Ignore(b => b.DomainEvents);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.CompanyId).IsRequired();

        builder.Property(x => x.OpeningTime)
            .HasConversion(timeOnly => timeOnly.ToString(),
                stringTimeOnly => stringTimeOnly == null
                    ? null
                    : TimeOnly.Parse(stringTimeOnly));

        builder.Property(x => x.ClosingTime)
            .HasConversion(timeOnly => timeOnly.ToString(),
                stringTimeOnly => stringTimeOnly == null
                    ? null
                    : TimeOnly.Parse(stringTimeOnly));

        builder.OwnsOne(b => b.ShopLocation, a =>
        {
            a.Property(x => x.Country).IsRequired();
            a.Property(x => x.City).IsRequired();
        });
    }
}