namespace TransitiveClosureTable.Application.Dto;

/// <summary>
/// DTO for creating a new node in a tree.
/// </summary>
public class CreateNodeRequestDto
{
    /// <summary>
    /// The ID of the tree where the new node will be added.
    /// </summary>
    public required int TreeId { get; set; }

    /// <summary>
    /// The name of the new node.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The ID of the parent node under which the new node will be attached.
    /// </summary>
    public required int ParentNodeId { get; set; }
}