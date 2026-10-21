using System.Text.Json.Serialization;

namespace rag.Models;

public class GeminiPart
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}