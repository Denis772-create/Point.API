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
        builder.Property(x => x.OpeningTime).HasColumnType("time(3)");
        builder.Property(x => x.ClosingTime).HasColumnType("time(3)");

        builder.OwnsOne(b => b.ShopLocation, a =>
        {
            a.Property(x => x.Country).IsRequired();
            a.Property(x => x.City).IsRequired();
        });
    }
}