using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[Route("api.user.tree.node")] // optional base route for clarity
[SwaggerTag("Represents tree node API")]
public class NodeController(INodeService nodeService) : ControllerBase
{
    /// <summary>
    /// Create a new node under a specified parent node.
    /// If parentNodeId is null, the node will be created as a root.
    /// </summary>
    [HttpPost("create")]
    [SwaggerOperation(Description = "Create a new node under a specified parent node. " +
                                    "If parentNodeId is null, the node will be created as a root.")]
    public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequestDto request)
    {
        var node = await nodeService.CreateAsync(request.Name, request.ParentNodeId);
        return CreatedAtAction(nameof(CreateNode), new { id = node.Id }, node);
    }

    /// <summary>
    /// Rename an existing node in the tree.
    /// The new name must be unique across all siblings.
    /// </summary>
    [HttpPut("rename")]
    [SwaggerOperation(Description = "Rename an existing node in your tree. " +
                                    "You must specify a node ID that belongs to your tree. " +
                                    "A new name of the node must be unique across all siblings.")]
    public async Task<IActionResult> RenameNode([FromBody] Node node)
    {
        var renamed = await nodeService.RenameAsync(node);
        return Ok(renamed); // Changed to Ok because renaming is an update, not create
    }

    /// <summary>
    /// Delete an existing node in the tree.
    /// </summary>
    [HttpDelete("delete")]
    [SwaggerOperation(Description = "Delete an existing node in your tree. " +
                                    "You must specify a node ID that belongs to your tree.")]
    public async Task<IActionResult> DeleteNode([FromRoute] int id)
    {
        var deleted = await nodeService.DeleteAsync(id);
        return Ok(deleted); // Changed to Ok because deletion is not a creation action
    }
}
