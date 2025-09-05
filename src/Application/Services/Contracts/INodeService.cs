using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

/// <summary>
///     Service interface for managing <see cref="Node"/> entities within a tree structure.
/// </summary>
public interface INodeService
{
    /// <summary>
    ///     Deletes a node by its ID.
    ///     The node must not be a root and must not have any child nodes.
    /// </summary>
    /// <param name="deleteNodeRequestDto">The request containing the ID of the node to delete.</param>
    /// <returns>
    ///     The deleted <see cref="Node"/> entity.
    /// </returns>
    Task<Node> DeleteAsync(DeleteNodeRequestDto deleteNodeRequestDto);

    /// <summary>
    ///     Renames the specified node.
    /// </summary>
    /// <param name="renameNodeRequestDto">The request containing the node ID and the new name.</param>
    /// <returns>
    ///     The renamed <see cref="Node"/> entity.
    /// </returns>
    Task<Node> RenameAsync(RenameNodeRequestDto renameNodeRequestDto);

    /// <summary>
    ///     Creates a new node within the tree structure.
    /// </summary>
    /// <param name="createNodeRequestDto">The request containing the details for the new node.</param>
    /// <returns>
    ///     The created <see cref="Node"/> entity.
    /// </returns>
    Task<Node> CreateAsync(CreateNodeRequestDto createNodeRequestDto);
}