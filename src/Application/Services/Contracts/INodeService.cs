using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

/// <summary>
///     Service interface for managing Node entities within a tree structure.
/// </summary>
public interface INodeService
{
    /// <summary>
    ///     Deletes a node by its ID.
    ///     The node must not be a root and must not have child nodes.
    /// </summary>
    /// <param name="id">The ID of the node to delete.</param>
    /// <returns>The deleted <see cref="Node" /> entity.</returns>
    Task<Node> DeleteAsync(int id);


    /// <summary>
    ///     Renames the specified node.
    /// </summary>
    /// <param name="id">The ID of the node to rename.</param>
    /// <param name="newName">The new name for the node.</param>
    /// <returns>The renamed <see cref="Node" /> entity.</returns>
    Task<Node> RenameAsync(int id, string newName);

    Task<Node> CreateAsync(string requestName, int requestParentNodeId);
}