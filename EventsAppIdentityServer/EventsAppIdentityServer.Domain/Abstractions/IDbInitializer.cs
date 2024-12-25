namespace EventsAppIdentityServer.Domain.Abstractions;

public interface IDbInitializer
{
    public Task InitializeDb();
}