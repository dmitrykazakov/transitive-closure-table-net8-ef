using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitiveClosureTable.Domain.Entities;

public class Tree
{
    [Key]
    public int Id { get; set; }

    [MaxLength(256)]
    public required string Name { get; set; }

    public ICollection<Node> Nodes { get; set; } = [];

    public ICollection<TransitiveClosure> TransitiveClosures { get; set; } = [];

    // Not persisted — only used in memory
    [NotMapped]
    public List<Node> NestedNodes { get; set; } = [];
}