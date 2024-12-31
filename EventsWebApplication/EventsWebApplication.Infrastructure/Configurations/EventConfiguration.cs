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

        builder.HasData(
            new
            {
                Id = 1,
                Title = "Tech Conference 2025",
                Description = "Join us for a day of insightful talks and networking with industry leaders.",
                EventDateTime = new DateTime(2025, 5, 5, 10, 0, 0, DateTimeKind.Utc),
                ParticipantsMaxCount = 1000,
                Image = (string?)null,
            },
            new
            {
                Id = 2,
                Title = "Art Workshop: Painting Basics",
                Description = "A beginner-friendly workshop to learn the fundamentals of painting.",
                EventDateTime = new DateTime(2025, 6, 15, 14, 0, 0, DateTimeKind.Utc),
                ParticipantsMaxCount = 850,
                Image = (string?)null,
            },
            new
            {
                Id = 3,
                Title = "Startup Pitch Night",
                Description = "Local startups pitch their ideas to a panel of investors.",
                EventDateTime = new DateTime(2025, 7, 20, 18, 30, 0, DateTimeKind.Utc),
                ParticipantsMaxCount = 320,
                Image = (string?)null,
            },
            new
            {
                Id = 4,
                Title = "Marathon Training Session",
                Description = "Get ready for the upcoming marathon with expert-led training.",
                EventDateTime = new DateTime(2025, 8, 10, 7, 0, 0, DateTimeKind.Utc),
                ParticipantsMaxCount = 500,
                Image = (string?)null,
            },
            new
            {
                Id = 5,
                Title = "Astronomy Night",
                Description = "An evening of stargazing and learning about the cosmos.",
                EventDateTime = new DateTime(2025, 10, 18, 20, 0, 0, DateTimeKind.Utc),
                ParticipantsMaxCount = 10,
                Image = (string?)null,
            });

        builder.OwnsOne(e => e.Category, categoryBuilder =>
        {
            categoryBuilder.Property(c => c.Name).HasMaxLength(100);
            categoryBuilder.Property(c => c.NormalizedName).HasMaxLength(100);

            categoryBuilder.HasData(
                new
                {
                    EventId = 1,
                    Name = "Technology",
                    NormalizedName = "TECHNOLOGY",
                },
                new
                {
                    EventId = 2,
                    Name = "Art",
                    NormalizedName = "ART",
                },
                new
                {
                    EventId = 3,
                    Name = "Business",
                    NormalizedName = "BUSINESS",
                },
                new
                {
                    EventId = 4,
                    Name = "Sports",
                    NormalizedName = "SPORTS",
                },
                new
                {
                    EventId = 5,
                    Name = "Science",
                    NormalizedName = "SCIENCE",
                });
        });

        builder.OwnsOne(e => e.Place, placeBuilder =>
        {
            placeBuilder.Property(c => c.Name).HasMaxLength(100);
            placeBuilder.Property(c => c.NormalizedName).HasMaxLength(100);

            placeBuilder.HasData(
                new
                {
                    EventId = 1,
                    Name = "Convention Center",
                    NormalizedName = "CONVENTION CENTER",
                },
                new
                {
                    EventId = 2,
                    Name = "Art Studio",
                    NormalizedName = "ART STUDIO",
                },
                new
                {
                    EventId = 3,
                    Name = "Startup Hub",
                    NormalizedName = "STARTUP HUB",
                },
                new
                {
                    EventId = 4,
                    Name = "City Park",
                    NormalizedName = "CITY PARK",
                },
                new
                {
                    EventId = 5,
                    Name = "Observatory",
                    NormalizedName = "OBSERVATORY",
                });
        });

    }
}
