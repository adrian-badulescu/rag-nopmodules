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

        // 1. Spunem lui EF Core să asigure existența extensiei pgvector în Postgres
        modelBuilder.HasPostgresExtension("vector");

        // 2. Configurăm dimensiunea vectorului pentru proprietatea Embedding
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