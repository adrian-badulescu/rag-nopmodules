using Microsoft.AspNetCore.Http.HttpResults;
using rag.Models;

namespace rag.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly IHttpClientService _httpClientService;

    public EmbeddingService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<float[]> GetEmbeddingAsync(string embeddedData)
    {
        var requestBody = new EmbeddingRequest
        {
            Model = "nomic-embed-text:latest",
            Prompt = embeddedData,
        };
        
        try
        {
            var httpClient = _httpClientService.GetHttpClient("LLMClient");
            var response = await httpClient.PostAsJsonAsync<EmbeddingRequest>("api/embeddings", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Embeddings API returned status code: {response.StatusCode}");
            }

            var result =  await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
            
            return result?.Embedding ?? throw new HttpRequestException("Failed to parse embedded response.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}