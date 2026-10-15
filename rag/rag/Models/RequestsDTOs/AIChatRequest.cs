using System.Text.Json.Serialization;

namespace rag.Models;

public class AIChatRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = "llama3";
    
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;
    
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;
}