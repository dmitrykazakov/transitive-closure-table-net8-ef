using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitiveClosureTable.Domain.Entities;

/// <summary>
///     Represents a node in a tree structure.
/// </summary>
public class Node
{
    /// <summary>
    ///     Primary key of the node.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    ///     Foreign key referencing the tree to which this node belongs.
    /// </summary>
    public int TreeId { get; set; }

    /// <summary>
    ///     The name of the node.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Navigation property to the parent tree.
    /// </summary>
    public Tree? Tree { get; set; }

    /// <summary>
    ///     List of child nodes in memory; not persisted in the database.
    /// </summary>
    [NotMapped]
    public List<Node> Ancestors { get; set; } = [];
}