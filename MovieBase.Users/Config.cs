using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;

namespace MovieBase.Users;

public static class Config
{
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            new ApiResource("MovieBaseAPI", "Movie Base API") { UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.Email }}
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope("MovieBaseAPI", "Movie Base API"),
            new ApiScope("openid", "OpenID Connect"),
            new ApiScope("profile", "Profil"),
            new ApiScope("email", "email")
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "MovieBaseClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "MovieBaseAPI"}
            },
            new Client
            {
                ClientId = "ResourceOwnerClient",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "openid", "profile", "email", "MovieBaseAPI"},
            }
        };
    }

    public static List<TestUser> GetUsers()
    {
        return new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "bob",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Role, "admin"),
                    new Claim(JwtClaimTypes.Name, "bob"),
                    new Claim(JwtClaimTypes.Email, "bob@bob.com")
                }
            }
        };
    }
}
