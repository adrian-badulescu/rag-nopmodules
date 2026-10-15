namespace rag.Models;

public class EndpointsConfig
{
    public LLMUrlConfig LLM { get; set; } = new();
    public NopModuleAppUrlConfig Nopmoduleapp { get; set; } = new();
}