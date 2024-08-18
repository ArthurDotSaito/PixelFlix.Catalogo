using System.Text;
using System.Text.Json;
using FC.Pixelflix.Catalogo.e2e.Extensions.String;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace FC.Pixelflix.Catalogo.e2e.Base;

class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}

public class ApiClient
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _defaultSerializerOptions;
    
    public ApiClient(HttpClient client)
    {
        _client = client;
        _defaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            PropertyNameCaseInsensitive = true
        };
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Post<TResponse>(string url, object request) where TResponse: class
    {
        var responseMessage = await _client.PostAsync(url, new StringContent(
            JsonSerializer.Serialize(request, _defaultSerializerOptions), Encoding.UTF8, "application/json"));

        var response = await ProcessResponse<TResponse>(responseMessage);

        return (responseMessage, response);
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Get<TResponse>(string route, object? queryStringParams = null) where TResponse: class
    {
        var url = PrepareQueryStringParams(route,  queryStringParams);
        var responseMessage = await _client.GetAsync(url);
        
        var response = await ProcessResponseAttributes<TResponse>(responseMessage);

        return (responseMessage, response);
    }

    public async Task<(HttpResponseMessage?, TResponse?)> Delete<TResponse>(string url) where TResponse: class
    {
        var responseMessage = await _client.DeleteAsync(url);

        var response = await ProcessResponse<TResponse>(responseMessage);

        return (responseMessage, response);
    }
    
    public async Task<(HttpResponseMessage?, TResponse?)> Put<TResponse>(string url, object request) where TResponse: class
    {
        var responseMessage = await _client.PutAsync(url, new StringContent(
            JsonSerializer.Serialize(request, _defaultSerializerOptions), Encoding.UTF8, "application/json"));

        var response = await ProcessResponseAttributes<TResponse>(responseMessage);

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
    
    private async Task<TResponse?> ProcessResponseAttributes<TResponse>(HttpResponseMessage responseMessage) where TResponse: class
    {
        var responseString = await responseMessage.Content.ReadAsStringAsync();
        
        TResponse? response = null;

        if (!String.IsNullOrWhiteSpace(responseString))
        {
            response = JsonSerializer.Deserialize<TResponse>(responseString, _defaultSerializerOptions);
        }

        return response;
    }
    
    private string PrepareQueryStringParams(string route, object? queryStringParams)
    {
        if(queryStringParams == null)
            return route;
        
        var jsonParameters = JsonSerializer.Serialize(queryStringParams);
        
        var jObject = JObject.Parse(jsonParameters);
        
        var filteredJObject = new JObject(jObject.Properties()
            .Where(prop => !string.IsNullOrEmpty(prop.Value.ToString()) && prop.Value.ToString() != "0"));
        
        var filteredParams = filteredJObject.ToObject<Dictionary<string, string>>();

        return QueryHelpers.AddQueryString(route, filteredParams!);
    }
    
}