using Microsoft.Extensions.Options;
using rag.Models;

namespace rag.Services;

public class ChatService : IChatService
{
    private readonly IHttpClientService  _httpClientService;
    private readonly LLMConfig _options;
    private readonly IDataRetrievalService _retrievalService;
    
    
    public ChatService(IHttpClientService  httpClientService,  IOptions<LLMConfig> options, IDataRetrievalService retrievalService)
    {
        _httpClientService = httpClientService;
        _options = options.Value;
        _retrievalService = retrievalService;
    }

    public async Task<string> GetAiResponseAsync(string prompt)
    {
        List<string> relevantChunks = await _retrievalService.GetRelevantContextAsync(prompt, maxResults: 3);
        string dbResult = string.Join("\n---\n", relevantChunks);
        
        string promptFinal = string.Format(_options.PromptTemplate, dbResult, prompt);
        
        AIChatRequest request = new AIChatRequest
        {
            Model = _options.LLMModel,
            Prompt = promptFinal,
            Stream = _options.LLMResponseMode.Stream
        };
        
        try
        {
            var httpClient = _httpClientService.GetHttpClient("LLMClient");
            var response = await httpClient.PostAsJsonAsync("api/generate", request);
            
            if (!response.IsSuccessStatusCode)
            {
                return "Error in communication with the LLM server.";
            }
            
            var result =  await response.Content.ReadFromJsonAsync<OllamaResponse>();
            return result?.Response ?? "No response returned from the LLM server.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.Message}");
            throw;
        }
    }
}