namespace rag.Services;

public interface IDataRetrievalService
{
    Task<List<string>> GetRelevantContextAsync(string userQuery, int maxResults = 3);
}