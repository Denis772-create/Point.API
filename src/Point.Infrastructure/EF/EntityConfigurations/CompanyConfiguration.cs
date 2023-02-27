namespace Point.Infrastructure.EF.EntityConfigurations;

public class CompanyConfiguration
    : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.Ignore(b => b.DomainEvents);

        builder.HasKey(e => e.Id);
            
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.Name).IsRequired();

        var navigation = builder.Metadata.FindNavigation(nameof(Company.Shops));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}