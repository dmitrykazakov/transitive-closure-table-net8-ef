using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;
using static NpgsqlTypes.NpgsqlTsQuery;

namespace TransitiveClosureTable.Application.Services;

/// <summary>
///     Service for managing Node entities and their transitive closure relationships.
/// </summary>
public class NodeService(IUnitOfWorkFactory unitOfWorkFactory) : INodeService
{
    /// <summary>
    ///     Creates a new node in a tree, optionally as a child of an existing node.
    /// </summary>
    /// <param name="name">The name of the new node.</param>
    /// <param name="parentNodeId">Optional parent node ID to attach the new node to.</param>
    /// <returns>The newly created Node entity.</returns>
    public async Task<Node> CreateAsync(string name, int parentNodeId)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        await using var transaction = await unitOfWork.BeginTransactionAsync();

        var parentNode = await unitOfWork.Nodes.GetByIdAsync(parentNodeId) ??
                         throw new SecureException($"Parent node with id {parentNodeId} not found");

        var treeId = parentNode.TreeId;

        // Create the new node
        var node = new Node
        {
            Name = name,
            TreeId = treeId
        };

        await unitOfWork.Nodes.AddAsync(node);
        await unitOfWork.CommitAsync(); // ensure node.Id is generated

        // Add ancestors and descendants entries
        await unitOfWork.TransitiveClosures.AddAsync(node.Id, parentNodeId);
        await unitOfWork.CommitAsync();

        await transaction.CommitAsync();
        return node;
    }

    /// <summary>
    ///     Deletes a node if it is not a root and has no child nodes.
    ///     Deletes all closure table rows referencing this node.
    /// </summary>
    /// <param name="id">The ID of the node to delete.</param>
    /// <returns>The deleted Node entity.</returns>
    /// <exception cref="SecureException">
    ///     Thrown if attempting to delete a root node or a node that has children.
    /// </exception>
    public async Task<Node> DeleteAsync(int id)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        var node = await unitOfWork.Nodes.GetByIdAsync(id);

        if (!await unitOfWork.Nodes.HasDirectAncestorAsync(id))
            throw new SecureException("Cannot delete the Root node.");

        if (await unitOfWork.Nodes.HasDirectDescendantAsync(id))
            throw new SecureException("You must delete all child nodes first.");

        // 1. Delete closure rows
        var relatedClosures = await unitOfWork.TransitiveClosures.GetAllByNodeIdAsync(id);

        await unitOfWork.TransitiveClosures.RemoveRangeAsync(relatedClosures);

        // Save closures FIRST so they are gone from the change tracker
        await unitOfWork.CommitAsync();

        // 2. Delete node itself
        await unitOfWork.Nodes.DeleteAsync(node);

        // Save node
        await unitOfWork.CommitAsync();

        // 3. Commit transaction
        await transaction.CommitAsync();

        return node;
    }

    /// <summary>
    ///     Renames an existing node.
    /// </summary>
    /// <param name="node">The node entity to rename.</param>
    /// <returns>The renamed Node entity.</returns>
    public Task<Node> RenameAsync(Node node)
    {
        throw new NotImplementedException();
    }
}