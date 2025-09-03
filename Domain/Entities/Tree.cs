using System.ComponentModel.DataAnnotations;

namespace TransitiveClosureTable.Domain.Entities;

/// <summary>
/// Represents a hierarchical tree structure containing nodes and their transitive closures.
/// </summary>
public class Tree
{
    /// <summary>
    /// Primary key of the tree.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Name of the tree (max length 256 characters).
    /// </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary>
    /// Collection of nodes that belong to this tree.
    /// </summary>
    public ICollection<Node> Nodes { get; set; } = [];

    /// <summary>
    /// Collection of transitive closure entries for this tree.
    /// </summary>
    public ICollection<TransitiveClosure> TransitiveClosures { get; set; } = [];
}