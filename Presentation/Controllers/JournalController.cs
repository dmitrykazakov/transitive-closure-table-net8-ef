using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[SwaggerTag("Represents journal API")]
public class JournalController : ControllerBase
{
    [HttpPost("api.user.journal.getRange")]
    [SwaggerOperation(Description =
        "Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.")]
    public IActionResult GetRange()
    {
        return Ok();
    }

    [HttpPost("api.user.journal.getSingle")]
    [SwaggerOperation(Description = "Returns the information about an particular event by ID.")]
    public IActionResult GetSingle()
    {
        return Ok();
    }
}