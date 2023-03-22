namespace Point.Infrastructure.EF.EntityConfigurations;

public class CardConfiguration
    : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards");
        builder.Property(cr => cr.CardNumber).HasMaxLength(14);


        builder.OwnsOne(c => c.QrCode, a =>
        {
	        a.Property(x => x.Code)
		        .IsRequired()
		        .HasMaxLength(2500);
        });
    }
}