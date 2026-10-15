namespace rag.Models;

public class LLMConfig
{
    public string LLMModel { get; set; } = string.Empty;
    
    public string PromptTemplate { get; set; } = string.Empty;

    public LLMResponseMode  LLMResponseMode { get; set; }
}

public class LLMResponseMode
{
    public bool Stream { get; set; } =  false;
}