using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace EventsAppIdentityServer.API.Utils;

public static class UtilsProvider
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
            //    AllowedScopes = {
            //        "events",
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.Profile,
            //        IdentityServerConstants.StandardScopes.Email,
            //    },
            //    RedirectUris = { "https://localhost:7096/signin-oidc" },
            //    PostLogoutRedirectUris = { "https://localhost:7096/signout-callback-oidc" }
            //}

            new Client
            {
                ClientId = "react_client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // ROPC
                ClientSecrets = { new Secret("react_secret".Sha256()) },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600, // 1 час
                RefreshTokenUsage = TokenUsage.ReUse,
                AllowedScopes = { "openid", "profile", "offline_access", "events_scope" },


                 // Важная настройка для использования куков
                //AllowAccessTokensViaBrowser = false,
            }

        };
}
