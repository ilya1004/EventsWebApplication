namespace EventsAppIdentityServer.Domain.Abstractions;

public interface IDbInitializer
{
    Task InitializeDb();
}