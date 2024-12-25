namespace EventsAppIdentityServer.Domain.Abstractions;

public interface ITokenValidationService
{
    public void ValidateAuthHeader(string? authHeader, string userId);
}
