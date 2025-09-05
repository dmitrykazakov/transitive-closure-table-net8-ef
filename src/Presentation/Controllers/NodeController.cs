using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Presentation.Controllers;

/// <summary>
/// API controller for managing tree nodes.
/// Supports creation, renaming, and deletion of nodes.
/// </summary>
[ApiController]
[SwaggerTag("Represents tree node API")]
public class NodeController(INodeService nodeService) : ControllerBase
{
    /// <summary>
    /// Creates a new node under a specified parent node.
    /// </summary>
    /// <remarks>
    /// If <paramref name="createNodeRequestDto.ParentNodeId"/> is null, the node will be created as a root.
    /// </remarks>
    /// <param name="createNodeRequestDto">DTO containing the tree ID, node name, and optional parent node ID.</param>
    /// <returns>
    /// Returns <see cref="CreatedAtActionResult"/> with the newly created node.
    /// </returns>
    [HttpPost("api.user.tree.node.create")]
    [SwaggerOperation(Description = "Create a new node under a specified parent node. " +
                                    "If parentNodeId is null, the node will be created as a root.")]
    public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequestDto createNodeRequestDto)
    {
        var node = await nodeService.CreateAsync(createNodeRequestDto);
        return CreatedAtAction(nameof(CreateNode), new { id = node.Id }, node);
    }

    /// <summary>
    /// Renames an existing node in the tree.
    /// </summary>
    /// <remarks>
    /// The new name must be unique among the node's siblings within the same tree.
    /// </remarks>
    /// <param name="renameNodeRequestDto">DTO containing the node ID and the new name.</param>
    /// <returns>
    /// Returns <see cref="OkObjectResult"/> with the renamed node.
    /// </returns>
    [HttpPost("api.user.tree.node.rename")]
    [SwaggerOperation(Description = "Rename an existing node in your tree. " +
                                    "You must specify a node ID that belongs to your tree. " +
                                    "A new name of the node must be unique across all siblings.")]
    public async Task<IActionResult> RenameNode([FromBody] RenameNodeRequestDto renameNodeRequestDto)
    {
        var renamed = await nodeService.RenameAsync(renameNodeRequestDto);
        return Ok(renamed);
    }

    /// <summary>
    /// Deletes an existing node from the tree.
    /// </summary>
    /// <remarks>
    /// The node can only be deleted if it is not a root and has no child nodes.
    /// </remarks>
    /// <param name="deleteNodeRequestDto">DTO containing the ID of the node to delete.</param>
    /// <returns>
    /// Returns <see cref="OkObjectResult"/> with the deleted node.
    /// </returns>
    [HttpPost("api.user.tree.node.delete")]
    [SwaggerOperation(Description = "Delete an existing node in your tree. " +
                                    "You must specify a node ID that belongs to your tree.")]
    public async Task<IActionResult> DeleteNode([FromBody] DeleteNodeRequestDto deleteNodeRequestDto)
    {
        var deleted = await nodeService.DeleteAsync(deleteNodeRequestDto);
        return Ok(deleted);
    }
}
