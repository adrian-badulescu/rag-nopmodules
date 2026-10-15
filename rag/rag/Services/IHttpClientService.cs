namespace rag.Services;

public interface IHttpClientService
{
    HttpClient GetHttpClient(string clientName);
}