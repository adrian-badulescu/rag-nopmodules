namespace rag.Services;

public interface IEmbeddingService
{
    Task<float[]> GetEmbeddingAsync(string embeddedData);
    Task<List<float[]>> GetEmbeddingsAsync(IEnumerable<string> dataToEmbed);
}