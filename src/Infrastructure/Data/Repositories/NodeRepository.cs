using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

/// <summary>
///     Repository implementation for performing CRUD operations on <see cref="Node" /> entities.
/// </summary>
public class NodeRepository(AppDbContext appDbContext) : INodeRepository
{
    /// <summary>
    ///     Retrieves a node by its ID.
    ///     Throws <see cref="SecureException" /> if the node does not exist.
    /// </summary>
    /// <param name="id">The ID of the node.</param>
    /// <returns>The node with the specified ID.</returns>
    public async Task<Node> GetByIdAsync(int id)
    {
        var node = await appDbContext.Nodes
                       .SingleOrDefaultAsync(n => n.Id == id) ??
                   throw new SecureException($"Node with ID = {id} not found.");
        return node;
    }

    /// <summary>
    ///     Deletes a node from the database.
    /// </summary>
    /// <param name="node">The node to delete.</param>
    public Task DeleteAsync(Node node)
    {
        appDbContext.Nodes.Remove(node);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Adds a new node to the database.
    /// </summary>
    /// <param name="node">The node to add.</param>
    public async Task AddAsync(Node node)
    {
        await appDbContext.Nodes.AddAsync(node);
    }

    /// <summary>
    ///     Retrieves all nodes belonging to a specific tree.
    /// </summary>
    /// <param name="treeId">The tree ID.</param>
    /// <returns>List of nodes in the tree.</returns>
    public async Task<List<Node>> GetByTreeIdAsync(int treeId)
    {
        return await appDbContext.Nodes.Where(n => n.TreeId == treeId).ToListAsync();
    }

    /// <summary>
    ///     Checks if the node has any direct children (depth = 1 in closure table).
    /// </summary>
    /// <param name="nodeId">The node ID to check.</param>
    /// <returns>True if the node has at least one direct child; otherwise, false.</returns>
    public async Task<bool> HasDirectDescendantAsync(int nodeId)
    {
        return await appDbContext.TransitiveClosures.AnyAsync(tc => tc.AncestorId == nodeId && tc.Depth == 1);
    }

    /// <summary>
    ///     Checks if the node has any direct parent (depth = 1 in closure table).
    /// </summary>
    /// <param name="nodeId">The node ID to check.</param>
    /// <returns>True if the node has at least one direct parent; otherwise, false.</returns>
    public async Task<bool> HasDirectAncestorAsync(int nodeId)
    {
        return await appDbContext.TransitiveClosures
            .AnyAsync(tc => tc.DescendantId == nodeId && tc.Depth == 1);
    }

    /// <summary>
    ///     Renames the specified node entity in the database.
    /// </summary>
    /// <param name="node">The node entity with updated name.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RenameAsync(Node node)
    {
        ArgumentNullException.ThrowIfNull(node, nameof(node));

        // Attach the entity if it's not being tracked
        if (appDbContext.Nodes.Local.All(n => n.Id != node.Id)) appDbContext.Nodes.Attach(node);

        // Mark entity as modified
        appDbContext.Entry(node).State = EntityState.Modified;

        // Do not call SaveChangesAsync() here if the UnitOfWork handles commits
        return Task.CompletedTask;
    }
}