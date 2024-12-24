using EventsAppIdentityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsAppIdentityServer.Infrastructure.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.Name).HasMaxLength(200);

        builder.Property(u => u.Surname).HasMaxLength(200);
    }
}
