namespace rag.Services;

public class HttpClientService :  IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient GetHttpClient(string clientName)
    {
        var client = _httpClientFactory.CreateClient(clientName);
        return client;
    }
    
}