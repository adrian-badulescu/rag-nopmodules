using System.Text.Json.Serialization;

namespace rag.Models;

public class GeminiChatResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; } = [];
}