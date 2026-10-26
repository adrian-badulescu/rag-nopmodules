using Microsoft.EntityFrameworkCore;
using rag.Models.Db;

namespace rag.Data;

public class RagDbContext : DbContext
{
    public RagDbContext(DbContextOptions<RagDbContext> options) : base(options) { }

    public DbSet<TextChunk> TextChunks => Set<TextChunk>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<TextChunk>(entity =>
        {
            entity.Property(b => b.Embedding)
                .HasColumnType("vector(768)"); 
            
            entity.HasIndex(b => b.Embedding)
                .HasMethod("hnsw")
                .HasOperators("vector_cosine_ops");
        });
    }
}