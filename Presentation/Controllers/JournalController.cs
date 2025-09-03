using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[Route("api.user.journal")] // Optional: base route for clarity
[SwaggerTag("Represents journal API")]
public class JournalController : ControllerBase
{
    /// <summary>
    /// Provides the pagination API.
    /// Skip = number of items to skip by server.
    /// Take = maximum number of items to return.
    /// All filter fields are optional.
    /// </summary>
    [HttpPost("getRange")]
    [SwaggerOperation(Description =
        "Provides the pagination API. Skip means the number of items should be skipped by server. " +
        "Take means the maximum number items should be returned by server. All fields of the filter are optional.")]
    public IActionResult GetRange()
    {
        // TODO: implement fetching logs with skip/take & filters
        return Ok();
    }

    /// <summary>
    /// Returns information about a particular event by ID.
    /// </summary>
    [HttpPost("getSingle")]
    [SwaggerOperation(Description = "Returns the information about a particular event by ID.")]
    public IActionResult GetSingle([FromBody] long eventId)
    {
        // TODO: implement fetching a single event by ID
        return Ok();
    }
}