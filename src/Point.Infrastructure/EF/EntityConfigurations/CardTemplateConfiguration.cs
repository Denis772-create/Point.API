namespace Point.Infrastructure.EF.EntityConfigurations;

public class CardTemplateConfiguration
    : IEntityTypeConfiguration<CardTemplate>
{
    public void Configure(EntityTypeBuilder<CardTemplate> builder)
    {
        builder.ToTable("CardTemplates");
        builder.Property(cr => cr.Title).HasMaxLength(80);
        builder.Property(cr => cr.Description).HasMaxLength(100);
    }
}