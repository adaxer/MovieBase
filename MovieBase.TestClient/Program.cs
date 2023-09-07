using IdentityModel.Client;

namespace MovieBase.TestClient;

internal class Program
{
    static async Task Main(string[] args)
    {
        await Console.Out.WriteLineAsync("Los gehts");
        Console.ReadLine();
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

        // Consume the MovieBase API
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
