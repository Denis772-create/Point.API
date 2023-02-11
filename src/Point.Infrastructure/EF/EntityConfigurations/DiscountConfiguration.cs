namespace Point.Infrastructure.EF.EntityConfigurations;

public class DiscountConfiguration
    : IEntityTypeConfiguration<Discount>

{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discounts");

        builder.Ignore(b => b.DomainEvents);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.StartDate).IsRequired(false);
        builder.Property(x => x.ExpirationDate).IsRequired(false);

    }
}

