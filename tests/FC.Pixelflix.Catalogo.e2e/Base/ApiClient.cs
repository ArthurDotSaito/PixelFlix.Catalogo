using System.Text;
using System.Text.Json;

namespace FC.Pixelflix.Catalogo.e2e.Base;

public class ApiClient
{
    private readonly HttpClient _client;
    
    public ApiClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Post<TResponse>(string url, object request)
    {
        var responseMessage = await _client.PostAsync(url, new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        var responseString = await responseMessage.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<TResponse>(responseString, new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            });

        return (responseMessage, response);
    }
}