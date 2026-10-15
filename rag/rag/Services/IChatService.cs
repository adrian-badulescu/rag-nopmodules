namespace rag.Services;

public interface IChatService
{
    Task<string> GetAiResponseAsync(string prompt);
}