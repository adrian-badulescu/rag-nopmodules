using System.Text.Json.Serialization;

namespace rag.Models;

public class OllamaResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;

}