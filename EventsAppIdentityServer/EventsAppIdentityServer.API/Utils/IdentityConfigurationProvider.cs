using Duende.IdentityServer.Models;

namespace EventsAppIdentityServer.API.Utils;

public static class IdentityConfigurationProvider
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("events_scope", "Events Server"),
        };


    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "react_client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("react_secret".Sha256()) },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600,
                RefreshTokenUsage = TokenUsage.ReUse,
                AllowedScopes = { "openid", "profile", "offline_access", "events_scope" },
            }
        };
}
