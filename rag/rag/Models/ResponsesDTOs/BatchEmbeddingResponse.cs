using System.Text.Json.Serialization;

namespace rag.Models;

public class BatchEmbeddingResponse
{
    [JsonPropertyName("embeddings")]
    public List<EmbeddingData> Embeddings { get; set; } = [];
}