using System.Text.Json.Serialization;

namespace rag.Models;

public class GeminiContent
{
    [JsonPropertyName("parts")]
    public List<GeminiPart> Parts { get; set; } = [];
}