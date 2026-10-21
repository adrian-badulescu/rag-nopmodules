using System.Text.Json.Serialization;

namespace rag.Models;

public class EmbeddingData
{
    [JsonPropertyName("values")]
    public float[] Values { get; set; } = [];
}