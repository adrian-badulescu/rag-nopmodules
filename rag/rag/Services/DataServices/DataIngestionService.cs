using Pgvector;
using rag.Data;
using rag.Models.Db;

namespace rag.Services;

public class DataIngestionService : IDataIngestionService
{
    private readonly RagDbContext _dbContext;
    private readonly IEmbeddingService _embeddingService;

    public DataIngestionService(RagDbContext dbContext, IEmbeddingService embeddingService)
    {
        _dbContext = dbContext;
        _embeddingService = embeddingService;
    }

    public async Task IngestDocumentAsync(string documentName, string fullText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fullText))
            {
                return;
            }
            
            var chunks = SplitIntoChunks(fullText, chunkSize: 500, overlap: 100);
            
            foreach (var chunkText in chunks)
            {
                if (string.IsNullOrWhiteSpace(chunkText)) continue;
                
                float[] embeddingFloats = await _embeddingService.GetEmbeddingAsync(chunkText);
                
                var textChunk = new TextChunk
                {
                    Content = chunkText,
                    Document = documentName,
                    Embedding = new Vector(embeddingFloats) 
                };

                _dbContext.TextChunks.Add(textChunk);
            }
            
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    
    private List<string> SplitIntoChunks(string text, int chunkSize, int overlap)
    {
        var chunks = new List<string>();
        if (text.Length <= chunkSize)
        {
            chunks.Add(text);
            return chunks;
        }

        int i = 0;
        while (i < text.Length)
        {
            if (i + chunkSize >= text.Length)
            {
                chunks.Add(text.Substring(i));
                break;
            }

            chunks.Add(text.Substring(i, chunkSize));
            i += (chunkSize - overlap); 
        }

        return chunks;
    }
}