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

        var response = await ProcessResponse<TResponse>(responseMessage);

        return (responseMessage, response);
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Get<TResponse>(string url) where TResponse: class
    {
        var responseMessage = await _client.GetAsync(url);
        
        /*var responseString = await responseMessage.Content.ReadAsStringAsync();
        
        TResponse? response = null;

        if (!String.IsNullOrWhiteSpace(responseString))
        {
            response = JsonSerializer.Deserialize<TResponse>(responseString, new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            });

        }*/
        var response = await ProcessResponse<TResponse>(responseMessage);

        return (responseMessage, response);
    }

    public async Task<(HttpResponseMessage?, TResponse?)> Delete<TResponse>(string url) where TResponse: class
    {
        var responseMessage = await _client.DeleteAsync(url);
        
        /*var responseString = await responseMessage.Content.ReadAsStringAsync();
        
        TResponse? response = null;

        if (!String.IsNullOrWhiteSpace(responseString))
        {
            response = JsonSerializer.Deserialize<TResponse>(responseString, new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            });

        }*/

        var response = await ProcessResponse<TResponse>(responseMessage);

        return (responseMessage, response);
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Put<TResponse>(string url, object request) where TResponse: class
    {
        var responseMessage = await _client.PutAsync(url, new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        var response = await ProcessResponse<TResponse>(responseMessage);

        return (responseMessage, response);
    }
    
    private async Task<TResponse?> ProcessResponse<TResponse>(HttpResponseMessage responseMessage) where TResponse: class
    {
        var responseString = await responseMessage.Content.ReadAsStringAsync();
        
        TResponse? response = null;

        if (!String.IsNullOrWhiteSpace(responseString))
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseObject = JObject.Parse(responseString); 
                var responseData = responseObject["response"]?.ToString();  
                response = JsonSerializer.Deserialize<TResponse>(responseData!, new JsonSerializerOptions{
                    PropertyNameCaseInsensitive = true
                });   
            }
            else
            {
                response = JsonSerializer.Deserialize<TResponse>(responseString!, new JsonSerializerOptions{
                    PropertyNameCaseInsensitive = true
                }); 
            }
        }

        return response;
    }
    
}