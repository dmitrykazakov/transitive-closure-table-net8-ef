namespace TransitiveClosureTable.Application.Dto;

/// <summary>
///     Data Transfer Object representing a node in a tree, including its nested children.
/// </summary>
public class NodeDto
{
    /// <summary>
    ///     The unique identifier of the node.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The name of the node.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     List of child nodes (direct descendants) in a nested structure.
    /// </summary>
    public List<NodeDto> Ancestors { get; set; } = [];
}