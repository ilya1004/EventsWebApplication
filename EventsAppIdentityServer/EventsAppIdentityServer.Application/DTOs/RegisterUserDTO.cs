namespace EventsAppIdentityServer.Application.DTOs;

public record RegisterUserDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateOnly Birthday { get; set; }
}
