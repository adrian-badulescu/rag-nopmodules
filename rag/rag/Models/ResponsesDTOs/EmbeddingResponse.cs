using System.Text.Json.Serialization;

namespace rag.Models;

public class EmbeddingResponse
{
    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; } = [];
}