using System.Collections.Generic;
using IdentityServer4.Models;

namespace SmartHome.IdentityServer.Configurations
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("eventlogapi", "Event devices API"),
                new ApiScope("controlapi", "Control devices API")
            };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("eventlogapi", "Event devices API resource"),
            new ApiResource("controlapi", "Control devices API resource")
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "eventlogapi", "controlapi" }
                },
                new Client
                {
                    ClientId = "ro.client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "eventlogapi", "controlapi" }
                }
            };
    }
}