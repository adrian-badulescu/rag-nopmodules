namespace rag.Models;

public class IngestRequest
{
    public string DocumentName { get; set; } = string.Empty;
    public string FullText { get; set; } = string.Empty;
}