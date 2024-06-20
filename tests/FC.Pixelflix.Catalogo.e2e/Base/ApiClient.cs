using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace FC.Pixelflix.Catalogo.e2e.Base;

public class ApiClient
{
    private readonly HttpClient _client;
    
    public ApiClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Post<TResponse>(string url, object request) where TResponse: class
    {
        var responseMessage = await _client.PostAsync(url, new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
        
        var responseString = await responseMessage.Content.ReadAsStringAsync();
        
        TResponse? response = null;

        if (!String.IsNullOrWhiteSpace(responseString))
        {
            var responseObject = JObject.Parse(responseString);
            var responseContent = responseObject["response"]?.ToString();
            
            response = JsonSerializer.Deserialize<TResponse>(responseContent!, new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            });

        }

        return (responseMessage, response);
    }
}