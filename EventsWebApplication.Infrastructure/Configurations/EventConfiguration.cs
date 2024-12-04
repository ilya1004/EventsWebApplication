using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApplication.Infrastructure.Configurations;

internal class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title).HasMaxLength(100).IsRequired();

        builder.Property(e => e.Description).HasMaxLength(500);

        builder.Property(e => e.Image).HasMaxLength(200);

        builder.HasIndex(e => e.Id).IsUnique();

        builder.OwnsOne(e => e.Category, categoryBuilder =>
        {
            categoryBuilder.Property(c => c.Name).HasMaxLength(100);
            categoryBuilder.Property(c => c.NormalizedName).HasMaxLength(100);
        });

        builder.OwnsOne(e => e.Place, placeBuilder =>
        {
            placeBuilder.Property(c => c.Name).HasMaxLength(100);
            placeBuilder.Property(c => c.NormalizedName).HasMaxLength(100);
        });
    }
}
