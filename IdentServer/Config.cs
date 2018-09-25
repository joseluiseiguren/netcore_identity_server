using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentServer
{
    public class Config
    {
        //InMemory resources list
        public static IEnumerable<ApiResource>GetApiResources()
        {
            return new List<ApiResource>
            {
                //web api a securizar
                new ApiResource
                {
                    Name = "MyWebAPI",
                    Scopes =
                    {
                        //los clientes que tengan este scope, tendran acceso al endpoint de Employees
                        new Scope()
                        {
                            Name = "MyWebAPI.employee",
                            DisplayName = "Full access to API Employee"
                        },

                        //los clientes que tengan este scope, tendran acceso al endpoint de Customers
                        new Scope
                        {
                            Name = "MyWebAPI.customer",
                            DisplayName = "Read only access to API Customer"
                        }
                    }
                }
            };
        }

        //InMemory clients list
        public static IEnumerable<Client>GetClients()
        {
            return new List<Client>
            {
                //client credentials grant
                new Client
                {
                    ClientId = "joseph1",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // secret for authentication
                    ClientSecrets = { new Secret("pepe".Sha256()) },
                    
                    // scopes that client has access to
                    AllowedScopes = { "MyWebAPI.employee", "MyWebAPI.customer" },

                    //access token expiration lifetime (seconds)
                    AccessTokenLifetime = 60,

                    //el identity server va a devolver un access token (y no un ID del mismo), para que la webapi se encarge de validarlo sin consultar el IDS
                    AccessTokenType = AccessTokenType.Jwt
                },

                //client credentials grant
                new Client
                {
                    ClientId = "joseph2",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // secret for authentication
                    ClientSecrets = { new Secret("pepe".Sha256()) },
                    
                    // scopes that client has access to
                    AllowedScopes = { "MyWebAPI.customer" },

                    //access token expiration lifetime (seconds)
                    AccessTokenLifetime = 60,

                    //el identity server va a devolver un access token (y no un ID del mismo), para que la webapi se encarge de validarlo sin consultar el IDS
                    AccessTokenType = AccessTokenType.Jwt
                },

                //client for the resource owner password grant
                //the client would collect the user’s password somehow, and send it to the token service during the token request
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "APIEmployee" }
                },

                // Web Site MVC a proteger con login
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5003/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }

        //InMemory users list
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }

        //InMemory identity resources
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }
}
