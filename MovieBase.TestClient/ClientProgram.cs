using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace MovieBase.TestClient;

internal class ClientProgram
{
    static async Task Main(string[] args)
    {
        await Console.Out.WriteLineAsync("Los gehts");
       // Console.ReadLine();


        // Discover endpoints from the IdentityServer metadata
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5555");
        if (disco.IsError)
        {
            Console.WriteLine(disco.Error);
            return;
        }

        // Request token using client credentials (replace with your client's credentials)
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "MovieBaseClient",
            ClientSecret = "secret",
            Scope = "MovieBaseAPI"
        });

        if (tokenResponse.IsError)
        {
            Console.WriteLine(tokenResponse.Error);
            return;
        }

        Console.WriteLine($"Token: {tokenResponse.AccessToken}\n");


        // Zweite Variante: mit Username/Password

        client = new HttpClient();

        tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "ResourceOwnerClient",
            ClientSecret = "secret",
            UserName = "bob",
            Password = "password",
            Scope = "openid profile email MovieBaseAPI"
        }) ;

        if (tokenResponse.IsError)
        {
            Console.WriteLine($"Error: {tokenResponse.Error}");
        }
        else
        {
            Console.WriteLine($"Token: {tokenResponse.AccessToken}");
            var token = new JwtSecurityToken(tokenResponse.AccessToken);
            foreach (var item in token.Payload.Values)
            {
                Console.WriteLine($"{item}");
            }
        }

        var apiClient = new HttpClient();
        apiClient.SetBearerToken(tokenResponse.AccessToken);

        var response = await apiClient.GetAsync("https://localhost:7027/Movie/AmILoggedIn");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"API Request Error: {response.StatusCode}");
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {content}");
        }

        await Console.Out.WriteLineAsync("Thats it");


        Console.ReadLine();
    }
}
