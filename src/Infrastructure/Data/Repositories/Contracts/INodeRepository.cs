using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

/// <summary>
///     Repository interface for performing CRUD operations on <see cref="Node" /> entities.
/// </summary>
public interface INodeRepository
{
    /// <summary>
    ///     Retrieves a node by its unique identifier.
    /// </summary>
    /// <param name="nodeId">The ID of the node.</param>
    /// <returns>The node with the specified ID.</returns>
    Task<Node> GetByIdAsync(int nodeId);

    /// <summary>
    ///     Deletes the specified node from the database.
    /// </summary>
    /// <param name="node">The node to delete.</param>
    Task DeleteAsync(Node node);

    /// <summary>
    ///     Adds a new node to the database.
    /// </summary>
    /// <param name="node">The node to add.</param>
    Task AddAsync(Node node);

    /// <summary>
    ///     Retrieves all nodes belonging to a specific tree.
    /// </summary>
    /// <param name="treeId">The ID of the tree.</param>
    /// <returns>List of nodes in the tree.</returns>
    Task<List<Node>> GetByTreeIdAsync(int treeId);

    /// <summary>
    ///     Checks if the specified node has any direct child nodes.
    /// </summary>
    /// <param name="nodeId">The node ID to check.</param>
    /// <returns>True if the node has at least one direct child; otherwise, false.</returns>
    Task<bool> HasDirectDescendantAsync(int nodeId);

    /// <summary>
    ///     Checks if the specified node has any direct ancestor nodes (parent).
    /// </summary>
    /// <param name="id">The node ID to check.</param>
    /// <returns>True if the node has a direct ancestor; otherwise, false.</returns>
    Task<bool> HasDirectAncestorAsync(int id);

    /// <summary>
    ///     Updates the specified node in the database.
    /// </summary>
    /// <param name="node">The node entity with updated values.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// node);
    Task RenameAsync(Node node);
}