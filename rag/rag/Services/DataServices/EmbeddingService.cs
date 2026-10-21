using Microsoft.Extensions.Options;
using rag.Models;

namespace rag.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly IHttpClientService _httpClientService;
    private readonly LLMConfig _options;
    private readonly EndpointsConfig _endpoints;

    public EmbeddingService(IHttpClientService httpClientService, 
        IOptions<LLMConfig> options, 
        IOptions<EndpointsConfig> endpoints)
    {
        _httpClientService = httpClientService;
        _options = options.Value;
        _endpoints = endpoints.Value;
    }

    public async Task<float[]> GetEmbeddingAsync(string embeddedData)
    {
        var requestBody = new
        {
            model = _options.EmbeddingModel,
            taskType = _options.TaskType,
            content = new { parts = new[] { new { text = embeddedData } } }
        };
        
        try
        {
            var httpClient = _httpClientService.GetHttpClient("LLMClient");
            var response = await httpClient.PostAsJsonAsync(_endpoints.LLM.RelativePathEmbeddings, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Embeddings API returned status code: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<SingleEmbeddingResponse>();
            
            return result?.Embedding.Values ?? throw new HttpRequestException("Failed to parse embedded response.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    
    public async Task<List<float[]>> GetEmbeddingsAsync(IEnumerable<string> dataToEmbed)
    {
        var textList = dataToEmbed.ToList();
        if (!textList.Any()) return new();

        var results = new List<float[]>();
        
        var batches = textList.Chunk(100);

        try
        {
            var httpClient = _httpClientService.GetHttpClient("LLMClient");
            var batchEndpoint = _endpoints.LLM.RelativePathBatchEmbeddings; 

            foreach (var batch in batches)
            {
                
                var requests = batch.Select(t => new 
                {
                    model = _options.EmbeddingModel,
                    taskType = _options.TaskType,
                    content = new { parts = new[] { new { text = t } } }
                }).ToList();

                var payload = new { requests };

                // 2. Facem UN SINGUR apel HTTP pentru tot lotul de maxim 100 de elemente
                var response = await httpClient.PostAsJsonAsync(batchEndpoint, payload);
            
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Batch Embeddings API returned status code: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<BatchEmbeddingResponse>();
            
                if (result?.Embeddings != null)
                {
                    results.AddRange(result.Embeddings.Select(e => e.Values));
                }
            }

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in batch embeddings: {ex.Message}");
            throw;
        }
    }

}