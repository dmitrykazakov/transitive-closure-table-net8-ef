using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[SwaggerTag("Represents entire tree API")]
public class TreeController(ITreeService treeService) : ControllerBase
{
    [HttpPost("api.user.tree.get")]
    [SwaggerOperation(Description =
        "Returns your entire tree. If your tree doesn't exist it will be created automatically.")]
    public async Task<IActionResult> GetOrCreateTreeAsync(string name)
    {
        var tree = await treeService.GetOrCreateAsync(name);
        return Ok(tree);
    }
}