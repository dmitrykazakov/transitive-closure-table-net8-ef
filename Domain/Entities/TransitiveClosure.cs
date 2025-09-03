namespace TransitiveClosureTable.Domain.Entities;

/// <summary>
/// Represents a transitive closure entry for a tree node hierarchy.
/// Each entry defines an ancestor-descendant relationship with a specific depth.
/// </summary>
public class TransitiveClosure
{
    /// <summary>
    /// Foreign key referencing the tree to which this closure belongs.
    /// </summary>
    public int TreeId { get; set; }

    /// <summary>
    /// Navigation property to the tree.
    /// </summary>
    public Tree? Tree { get; set; }

    /// <summary>
    /// ID of the ancestor node.
    /// </summary>
    public int AncestorId { get; set; }

    /// <summary>
    /// Navigation property to the ancestor node.
    /// </summary>
    public Node? Ancestor { get; set; }

    /// <summary>
    /// ID of the descendant node.
    /// </summary>
    public int DescendantId { get; set; }

    /// <summary>
    /// Navigation property to the descendant node.
    /// </summary>
    public Node? Descendant { get; set; }

    /// <summary>
    /// The depth of the ancestor-descendant relationship.
    /// 0 indicates a self-reference; 1 indicates a direct parent-child relationship, etc.
    /// </summary>
    public int Depth { get; set; }
}