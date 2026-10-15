using System.ComponentModel.DataAnnotations;
using Pgvector;

namespace rag.Models.Db;

public class TextChunk
{
    [Key]
    public int Key { get; set; } = 0;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public string Document { get; set; } = string.Empty;

    [Required] public Vector? Embedding { get; set; }
}