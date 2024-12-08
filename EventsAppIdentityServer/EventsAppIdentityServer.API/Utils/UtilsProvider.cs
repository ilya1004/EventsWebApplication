﻿using Duende.IdentityServer;
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
            new ApiScope(name: "events_scope", "Events Server"),
        };


    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "events",
                ClientSecrets = { new Secret("secret".Sha256()) },
                //AllowedGrantTypes = GrantTypes.Code,
                //AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600, // 1 час
                AbsoluteRefreshTokenLifetime = 2592000, // 30 дней

                AllowedScopes = {
                    "events",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                },
                RedirectUris = { "https://localhost:7096/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7096/signout-callback-oidc" }
            }
        };
}
