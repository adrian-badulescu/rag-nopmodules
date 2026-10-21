using System.Text.Json.Serialization;

namespace rag.Models;

public class SingleEmbeddingResponse
{
    [JsonPropertyName("embedding")]
    public EmbeddingData Embedding { get; set; } = new();
}