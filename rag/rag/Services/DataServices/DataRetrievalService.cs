using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using rag.Data;

namespace rag.Services;

public class DataRetrievalService : IDataRetrievalService
{
    private readonly RagDbContext _dbContext;
    private readonly IEmbeddingService _embeddingService;

    public DataRetrievalService(RagDbContext dbContext, IEmbeddingService embeddingService)
    {
        _dbContext = dbContext;
        _embeddingService = embeddingService;
    }
    
    public async Task<List<string>> GetRelevantContextAsync(string userQuery, int maxResults = 3)
    {
        float[] queryFloats = await _embeddingService.GetEmbeddingAsync(userQuery);
        
        var queryVector = new Vector(queryFloats);

        var relevantChunks = await _dbContext.TextChunks
            .OrderBy(c => c.Embedding.CosineDistance(queryVector)) 
            .Take(maxResults)
            .Select(c => c.Content)
            .ToListAsync();

        return relevantChunks;
    }
}