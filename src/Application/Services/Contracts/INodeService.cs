using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

/// <summary>
/// Service interface for managing Node entities within a tree structure.
/// </summary>
public interface INodeService
{
    /// <summary>
    /// Deletes a node by its ID.
    /// The node must not be a root and must not have child nodes.
    /// </summary>
    /// <param name="id">The ID of the node to delete.</param>
    /// <returns>The deleted <see cref="Node"/> entity.</returns>
    Task<Node> DeleteAsync(int id);

    /// <summary>
    /// Renames an existing node.
    /// </summary>
    /// <param name="node">The node entity to rename.</param>
    /// <returns>The renamed <see cref="Node"/> entity.</returns>
    Task<Node> RenameAsync(Node node);

    /// <summary>
    /// Creates a new node in the specified tree, optionally as a child of an existing node.
    /// </summary>
    /// <param name="treeId">The tree ID where the node will be added.</param>
    /// <param name="name">The name of the new node.</param>
    /// <param name="parentNodeId">Optional parent node ID to attach the new node to.</param>
    /// <returns>The newly created <see cref="Node"/> entity.</returns>
    Task<Node> CreateAsync(int treeId, string name, int? parentNodeId = null);
}