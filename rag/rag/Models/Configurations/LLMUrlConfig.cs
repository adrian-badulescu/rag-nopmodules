namespace rag.Models;

public class LLMUrlConfig
{
    public string BaseURL { get; set; } = string.Empty;
    
    public string RelativePathChat { get; set; } = string.Empty;    
    public string RelativePathEmbeddings { get; set; } = string.Empty;
    public string RelativePathBatchEmbeddings { get; set; } = string.Empty;
    
    public string ApiKey { get; set; } = string.Empty;
}

