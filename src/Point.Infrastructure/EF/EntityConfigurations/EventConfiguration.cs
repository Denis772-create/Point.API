namespace Point.Infrastructure.EF.EntityConfigurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Discriminator).HasMaxLength(64);
        builder.Property(x => x.EntityId).IsRequired();
        builder.Property(x => x.SerializedValue).IsRequired();
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.Timestamp);
    }
}