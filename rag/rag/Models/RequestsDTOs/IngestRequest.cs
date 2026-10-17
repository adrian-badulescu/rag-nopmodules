using System.Text.Json.Serialization;

namespace rag.Models;

public class IngestRequest
{
    [JsonPropertyName("documentName")]
    public string DocumentName { get; set; } = string.Empty;
    
    [JsonPropertyName("fullText")]
    public string FullText { get; set; } = string.Empty;
}