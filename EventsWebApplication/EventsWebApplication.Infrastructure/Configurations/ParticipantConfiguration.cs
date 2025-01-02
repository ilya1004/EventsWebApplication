using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApplication.Infrastructure.Configurations;

internal class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Email).HasMaxLength(255).IsRequired();

        builder.HasIndex(p => p.Id).IsUnique();

        builder.HasData(
            new
            {
                Id = 1,
                Email = "ilya@gmail.com",
                EventId = 1,
                EventRegistrationDate = new DateTime(2024, 11, 20, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 2,
                Email = "ilya@gmail.com",
                EventId = 2,
                EventRegistrationDate = new DateTime(2024, 12, 19, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 3,
                Email = "ilya@gmail.com",
                EventId = 3,
                EventRegistrationDate = new DateTime(2024, 10, 10, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 4,
                Email = "ilya@gmail.com",
                EventId = 4,
                EventRegistrationDate = new DateTime(2024, 9, 15, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 5,
                Email = "ilya@gmail.com",
                EventId = 5,
                EventRegistrationDate = new DateTime(2024, 9, 15, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 6,
                Email = "anna@gmail.com",
                EventId = 1,
                EventRegistrationDate = new DateTime(2024, 8, 19, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 7,
                Email = "anna@gmail.com",
                EventId = 3,
                EventRegistrationDate = new DateTime(2024, 11, 14, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 8,
                Email = "dmitry@gmail.com",
                EventId = 2,
                EventRegistrationDate = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = 9,
                Email = "dmitry@gmail.com",
                EventId = 4,
                EventRegistrationDate = new DateTime(2024, 10, 13, 0, 0, 0, DateTimeKind.Utc),
            });

        builder.OwnsOne(p => p.Person, personBuilder =>
        {
            personBuilder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            personBuilder.Property(p => p.Surname).HasMaxLength(200).IsRequired();
            personBuilder.Property(p => p.BirthdayDate).IsRequired();

            personBuilder.HasData(
                new
                {
                    ParticipantId = 1,
                    Name = "Ilya",
                    Surname = "Rabets",
                    BirthdayDate = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 2,
                    Name = "Ilya",
                    Surname = "Rabets",
                    BirthdayDate = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 3,
                    Name = "Ilya",
                    Surname = "Rabets",
                    BirthdayDate = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 4,
                    Name = "Ilya",
                    Surname = "Rabets",
                    BirthdayDate = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 5,
                    Name = "Ilya",
                    Surname = "Rabets",
                    BirthdayDate = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 6,
                    Name = "Anna",
                    Surname = "Petrova",
                    BirthdayDate = new DateTime(1995, 4, 23, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 7,
                    Name = "Anna",
                    Surname = "Petrova",
                    BirthdayDate = new DateTime(1995, 4, 23, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 8,
                    Name = "Dmitry",
                    Surname = "Ivanov",
                    BirthdayDate = new DateTime(1988, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    ParticipantId = 9,
                    Name = "Dmitry",
                    Surname = "Ivanov",
                    BirthdayDate = new DateTime(1988, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                });
        });


        builder.HasOne(p => p.Event)
            .WithMany(e => e.Participants)
            .HasForeignKey(p => p.EventId);
    }
}
