using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitiveClosureTable.Domain.Entities;

public class Node
{
    [Key]
    public int Id { get; set; }
    public int TreeId { get; set; } 
    public required string Name { get; set; }

    public Tree? Tree { get; set; }

    // Not persisted — only used in memory
    [NotMapped] public List<Node> Ancestors { get; set; } = [];
}