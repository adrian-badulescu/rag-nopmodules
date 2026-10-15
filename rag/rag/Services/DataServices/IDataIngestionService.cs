namespace rag.Services;

public interface IDataIngestionService
{
    Task IngestDocumentAsync(string documentName, string fullText);
}