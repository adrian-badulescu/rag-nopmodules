namespace rag.Services;

public interface IEmbeddingService
{
    Task<float[]> GetEmbeddingAsync(string embeddedData);
}