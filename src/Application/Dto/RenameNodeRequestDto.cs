namespace TransitiveClosureTable.Application.Dto;

/// <summary>
///     DTO for renaming an existing node in a tree.
/// </summary>
public class RenameNodeRequestDto
{
    /// <summary>
    ///     The ID of the node to rename.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///     The new name for the node.
    /// </summary>
    public required string Name { get; set; }
}