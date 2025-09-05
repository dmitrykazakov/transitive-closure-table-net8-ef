using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[Route("api.user.journal")] // optional base route for clarity
[SwaggerTag("Represents journal API")]
public class JournalController(IExceptionJournalService exceptionJournalService) : ControllerBase
{
    /// <summary>
    ///     Retrieves a paginated and optionally filtered list of exception journal entries.
    /// </summary>
    [HttpPost("getRange")]
    [SwaggerOperation(Description =
        "Provides the pagination API. Skip means the number of items should be skipped by server. " +
        "Take means the maximum number items should be returned by server. All fields of the filter are optional.")]
    public async Task<IActionResult> GetRange([FromBody] ExceptionJournalRequestDto request)
    {
        var entries = await exceptionJournalService.GetRangeAsync(request);

        // Optionally include total count for pagination (if needed)
        var totalCount = entries.Count; // Or implement service to return total count separately

        var response = new
        {
            TotalCount = totalCount,
            Items = entries
        };

        return Ok(response);
    }

    /// <summary>
    ///     Retrieves a single exception journal entry by its identifier.
    /// </summary>
    [HttpPost("getSingle")]
    [SwaggerOperation(Description = "Returns the information about a particular event by ID.")]
    public async Task<IActionResult> GetSingle([FromBody] long eventId)
    {
        var entry = await exceptionJournalService.GetSingleAsync(eventId);
        return entry is null ? NotFound() : Ok(entry);
    }
}