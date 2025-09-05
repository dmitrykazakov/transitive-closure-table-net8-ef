using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Dto;

/// <summary>
///     Data Transfer Object representing a tree and its root node.
/// </summary>
public class TreeDto
{
    /// <summary>
    ///     The unique identifier of the tree.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The name of the tree.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     The root node of the tree, including all nested child nodes.
    /// </summary>
    public required NodeDto Root { get; set; }
}

/// <summary>
///     Extension methods for mapping domain entities to DTOs.
/// </summary>
public static class TreeMappingExtensions
{
    /// <summary>
    ///     Converts a <see cref="Node" /> entity to a <see cref="NodeDto" />, including its nested children.
    /// </summary>
    /// <param name="node">The domain node entity.</param>
    /// <returns>A <see cref="NodeDto" /> with the same ID, name, and nested child nodes.</returns>
    public static NodeDto ToDto(this Node node)
    {
        return new NodeDto
        {
            Id = node.Id,
            Name = node.Name,
            Descendants = node.Descendants.Select(c => c.ToDto()).ToList()
        };
    }
}