using Microsoft.Extensions.Options;
using Pgvector;
using rag.Data;
using rag.Models;
using rag.Models.Db;

namespace rag.Services;

public class DataIngestionService : IDataIngestionService
{
    private readonly RagDbContext _dbContext;
    private readonly IEmbeddingService _embeddingService;
    private readonly LLMConfig _llmOptions;

    public DataIngestionService(RagDbContext dbContext,
        IEmbeddingService embeddingService, IOptions<LLMConfig> llmOptions)
    {
        _dbContext = dbContext;
        _embeddingService = embeddingService;
        _llmOptions = llmOptions.Value;
    }

    public async Task IngestDocumentAsync(string documentName, string fullText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fullText))
            {
                return;
            }

            int chunkSize = _llmOptions.TextChunkSize;
            int overlap = _llmOptions.TextOverlapSize;
            
            var chunks = SplitIntoChunks(fullText, chunkSize, overlap)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();
            
            if (!chunks.Any()) return;
            // groups database transaction number
            int batchSize = _llmOptions.IngestDocumentBatchSize;
            
            var chunkBatches = chunks.Chunk(batchSize);
            
            foreach (var batch in chunkBatches)
            {
                var batchEmbeddings = await _embeddingService.GetEmbeddingsAsync(batch);
                
                for (int i = 0; i < batch.Length; i++)
                {
                    var textChunk = new TextChunk
                    {
                        Content = batch[i],
                        Document = documentName,
                        Embedding = new Vector(batchEmbeddings[i])  
                    };

                    _dbContext.TextChunks.Add(textChunk);
                }
                await _dbContext.SaveChangesAsync();
                _dbContext.ChangeTracker.Clear();
            }
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