using Microsoft.Extensions.Options;
using rag.Models;

namespace rag.Services;

public class ChatService : IChatService
{
    private readonly IHttpClientService  _httpClientService;
    private readonly LLMConfig _options;
    private readonly EndpointsConfig _endpoints;
    private readonly IDataRetrievalService _retrievalService;
    
    
    public ChatService(IHttpClientService  httpClientService, 
        IOptions<LLMConfig> options, 
        IOptions<EndpointsConfig> endpoints, 
        IDataRetrievalService retrievalService)
    {
        _httpClientService = httpClientService;
        _options = options.Value;
        _endpoints = endpoints.Value;
        _retrievalService = retrievalService;
    }

    public async Task<string> GetAiResponseAsync(string prompt)
    {
        List<string> relevantChunks = await _retrievalService.GetRelevantContextAsync(prompt, maxResults: 3);
        string dbResult = string.Join("\n---\n", relevantChunks);
        
        string promptFinal = string.Format(_options.PromptTemplate, dbResult, prompt);
        
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = promptFinal }
                    }
                }
            }
        };
        
        try
        {
            var httpClient = _httpClientService.GetHttpClient("LLMClient");
            var response = await httpClient.PostAsJsonAsync(_endpoints.LLM.RelativePathEmbeddings, requestBody);
            
            if (!response.IsSuccessStatusCode)
            {
                return "Error in communication with the Google Gemini API.";
            }
            
            var result =  await response.Content.ReadFromJsonAsync<GeminiChatResponse>();
            var responseText = result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
            
            return responseText ?? "No response returned from the LLM model.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.Message}");
            throw;
        }
    }
}