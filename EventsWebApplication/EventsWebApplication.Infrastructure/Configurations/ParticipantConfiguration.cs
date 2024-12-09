using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Participants;
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

        builder.OwnsOne(p => p.Person, personBuilder =>
        {
            personBuilder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            personBuilder.Property(p => p.Surname).HasMaxLength(200).IsRequired();
            personBuilder.Property(p => p.BirthdayDate).IsRequired();
        });

        builder.HasOne(p => p.Event)
            .WithMany(e => e.Participants)
            .HasForeignKey(p => p.EventId);
    }
}
