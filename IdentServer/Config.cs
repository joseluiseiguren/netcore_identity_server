using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentServer
{
    public class Config
    {
        //InMemory resources list
        public static IEnumerable<ApiResource>GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("APICustomer", "API de los customers de ENCAMINA"),
                new ApiResource("APIEmployee", "API de los empleados de ENCAMINA")
            };
        }

        //InMemory clients list
        public static IEnumerable<Client>GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "joseph1",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // secret for authentication
                    ClientSecrets = { new Secret("pepe".Sha256()) },
                    
                    // scopes that client has access to
                    AllowedScopes = { "APIEmployee" }
                },

                new Client
                {
                    ClientId = "alice",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // secret for authentication
                    ClientSecrets = { new Secret("pepe".Sha256()) },
                    
                    // scopes that client has access to
                    AllowedScopes = { "APIEmployee" }
                }
            };
        }        
    }
}
