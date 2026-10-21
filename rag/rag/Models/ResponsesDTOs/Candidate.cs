using System.Text.Json.Serialization;

namespace rag.Models;

public class Candidate
{
    [JsonPropertyName("content")]
    public GeminiContent Content { get; set; } = new();
}