using System.ComponentModel.DataAnnotations;

namespace TransitiveClosureTable.Domain.Entities;

public class Tree
{
    [Key]
    public int Id { get; set; }

    [MaxLength(256)]
    public required string Name { get; set; }

    public ICollection<Node> Nodes { get; set; } = [];

    public ICollection<TransitiveClosure> TransitiveClosures { get; set; } = [];
}