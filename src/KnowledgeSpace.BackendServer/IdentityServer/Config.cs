using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace KnowledgeSpace.BackendServer.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[] {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[] {
                new ApiResource("api.knowledgespace", "KnowledgeSpace API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "webportal",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowOfflineAccess = true,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api.knowledgespace"
                    }
                },
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "Swagger Client",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5000/swagger/oauth2-redirect.html" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5000/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins = {"https://localhost:5000"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.knowledgespace"
                    }
                },
                new Client
                {
                    ClientId = "angular_admin",
                    ClientName = "Angular Admin",

                    AccessTokenType = AccessTokenType.Reference,

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    RequirePkce = true,

                    // where to redirect to after login
                    RedirectUris = new List<string>
                    { 
                        "https://localhost:4200",
                        "https://localhost:4200/authentication/login-callback",
                        "https://localhost:4200/silent-renew.html",
                    },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = new List<string>
                    { 
                        "https://localhost:4200/unauthorized", 
                        "https://localhost:4200/authentication/logout-callback", 
                        "https://localhost:4200", 
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:4200"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.knowledgespace"
                    }
                },
            };
    }
}
