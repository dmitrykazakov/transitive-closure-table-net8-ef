using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[SwaggerTag("Represents tree node API")]
public class NodeController(INodeService nodeService) : ControllerBase
{
    [HttpPost("api.user.tree.node.create")]
    [SwaggerOperation(Description = "Create a new node under a specified parent node. If parentNodeId is null, the node will be created as a root.")]
    public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequestDto request)
    {
        var node = await nodeService.CreateAsync(request.TreeId, request.Name, request.ParentNodeId);
        return CreatedAtAction(nameof(CreateNode), new { id = node.Id }, node);
    }

    [HttpPut("api.user.tree.node.rename")]
    [SwaggerOperation(Description = "Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.")]
    public async Task<IActionResult> RenameNode([FromBody] Node node)
    {
        var renamed = await nodeService.RenameAsync(node);
        return CreatedAtAction(nameof(RenameNode), new { id = renamed.Id }, renamed);
    }

    [HttpDelete("api.user.tree.node.delete")]
    [SwaggerOperation(Description = "Delete an existing node in your tree.You must specify a node ID that belongs your tree.")]
    public async Task<IActionResult> DeleteNode(int id)
    {
        var deleted = await nodeService.DeleteAsync(id);
        return CreatedAtAction(nameof(DeleteNode), new { id = deleted.Id }, deleted);
    }
}