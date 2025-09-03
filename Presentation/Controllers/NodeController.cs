using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[SwaggerTag("Represents tree node API")]
public class NodeController(INodeService nodeService) : ControllerBase
{
    [HttpPut("api.user.tree.node.rename")]
    [SwaggerOperation(Description =
        "Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.")]
    public async Task<IActionResult> RenameNode([FromBody] Node node)
    {
        var renamed = await nodeService.RenameAsync(node);
        return CreatedAtAction(nameof(RenameNode), new { id = renamed.Id }, renamed);
    }

    [HttpDelete("api.user.tree.node.delete")]
    [SwaggerOperation(Description =
        "Delete an existing node in your tree.You must specify a node ID that belongs your tree.")]
    public async Task<IActionResult> DeleteNode(int id)
    {
        await nodeService.DeleteAsync(id);
        return NoContent();
    }
}