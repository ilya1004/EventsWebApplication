using Microsoft.AspNetCore.Identity;

namespace EventsAppIdentityServer.Domain.Entities;

public class AppUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}
