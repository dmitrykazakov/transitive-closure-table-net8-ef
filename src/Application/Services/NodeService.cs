using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

/// <summary>
///     Provides operations for managing <see cref="Node"/> entities
///     and maintaining their transitive closure relationships.
/// </summary>
public class NodeService(IUnitOfWorkFactory unitOfWorkFactory) : INodeService
{
    /// <summary>
    ///     Creates a new node within an existing tree.
    ///     The node is attached as a child of the specified parent node.
    /// </summary>
    /// <param name="createNodeRequestDto">
    ///     The request containing the new node's name and the parent node ID.
    /// </param>
    /// <returns>
    ///     The newly created <see cref="Node"/> entity.
    /// </returns>
    /// <exception cref="SecureException">
    ///     Thrown if the specified parent node does not exist.
    /// </exception>
    public async Task<Node> CreateAsync(CreateNodeRequestDto createNodeRequestDto)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        await using var transaction = await unitOfWork.BeginTransactionAsync();

        var parentNode = await unitOfWork.Nodes.GetByIdAsync(createNodeRequestDto.ParentNodeId) ??
                         throw new SecureException($"Parent node with id {createNodeRequestDto.ParentNodeId} not found");

        var treeId = parentNode.TreeId;

        // Create the new node
        var node = new Node
        {
            Name = createNodeRequestDto.Name,
            TreeId = treeId
        };

        await unitOfWork.Nodes.AddAsync(node);
        await unitOfWork.CommitAsync(); // ensure node.Id is generated

        // Add ancestors and descendants entries
        await unitOfWork.TransitiveClosures.AddAsync(node.Id, createNodeRequestDto.ParentNodeId);
        await unitOfWork.CommitAsync();

        await transaction.CommitAsync();
        return node;
    }

    /// <summary>
    ///     Deletes a node from the tree.
    ///     The node must not be a root node and must not have any children.
    ///     All related closure table rows referencing this node are also removed.
    /// </summary>
    /// <param name="deleteNodeRequestDto">
    ///     The request containing the ID of the node to delete.
    /// </param>
    /// <returns>
    ///     The deleted <see cref="Node"/> entity.
    /// </returns>
    /// <exception cref="SecureException">
    ///     Thrown if attempting to delete a root node or a node with children.
    /// </exception>
    public async Task<Node> DeleteAsync(DeleteNodeRequestDto deleteNodeRequestDto)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        var node = await unitOfWork.Nodes.GetByIdAsync(deleteNodeRequestDto.Id);

        if (!await unitOfWork.Nodes.HasDirectAncestorAsync(deleteNodeRequestDto.Id))
            throw new SecureException("Cannot delete the Root node.");

        if (await unitOfWork.Nodes.HasDirectDescendantAsync(deleteNodeRequestDto.Id))
            throw new SecureException("You must delete all child nodes first.");

        // Delete closure rows
        var relatedClosures = await unitOfWork.TransitiveClosures.GetAllByNodeIdAsync(deleteNodeRequestDto.Id);
        await unitOfWork.TransitiveClosures.RemoveRangeAsync(relatedClosures);

        // Save closures so they are gone from the change tracker
        await unitOfWork.CommitAsync();

        // Delete node itself
        await unitOfWork.Nodes.DeleteAsync(node);

        // Save node deletion
        await unitOfWork.CommitAsync();

        await transaction.CommitAsync();

        return node;
    }

    /// <summary>
    ///     Renames an existing node.
    /// </summary>
    /// <param name="renameNodeRequestDto">
    ///     The request containing the node ID and the new name.
    /// </param>
    /// <returns>
    ///     The renamed <see cref="Node"/> entity.
    /// </returns>
    /// <exception cref="SecureException">
    ///     Thrown if the new name is empty or if the node is not found.
    /// </exception>
    public async Task<Node> RenameAsync(RenameNodeRequestDto renameNodeRequestDto)
    {
        if (string.IsNullOrWhiteSpace(renameNodeRequestDto.Name))
            throw new SecureException("New name must not be empty");

        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        // Load the node
        var node = await unitOfWork.Nodes.GetByIdAsync(renameNodeRequestDto.Id)
                   ?? throw new SecureException($"Node with id {renameNodeRequestDto.Id} not found");

        // Update the name
        node.Name = renameNodeRequestDto.Name;

        await unitOfWork.Nodes.RenameAsync(node);

        await unitOfWork.CommitAsync();
        await transaction.CommitAsync();

        return node;
    }
}
