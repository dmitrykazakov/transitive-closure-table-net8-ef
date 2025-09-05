using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Presentation.Controllers;

/// <summary>
///     Controller responsible for handling API requests related to trees.
///     Provides endpoints for retrieving and managing entire tree structures.
/// </summary>
[ApiController]
[SwaggerTag("API endpoints for managing trees.")]
public class TreeController(ITreeService treeService) : ControllerBase
{
    /// <summary>
    ///     Retrieves the entire tree by name. 
    ///     If a tree with the specified name does not exist, it will be created automatically.
    /// </summary>
    /// <param name="name">The name of the tree to retrieve or create.</param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the tree data in the response body.
    /// </returns>
    [HttpPost("api.user.tree.get")]
    [SwaggerOperation(
        Summary = "Retrieve or create a tree by name.",
        Description = "Returns your entire tree. " +
                      "If a tree with the specified name doesn't exist, it will be created automatically."
    )]
    public async Task<IActionResult> GetOrCreateTreeAsync([FromQuery] string name)
    {
        var tree = await treeService.GetOrCreateAsync(name);
        return Ok(tree);
    }
}