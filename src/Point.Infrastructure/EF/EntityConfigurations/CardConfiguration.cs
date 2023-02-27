namespace Point.Infrastructure.EF.EntityConfigurations;

public class CardConfiguration
    : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards");
        builder.Property(cr => cr.QrCode).HasMaxLength(300);
        builder.Property(cr => cr.CardNumber).HasMaxLength(20);
    }
}