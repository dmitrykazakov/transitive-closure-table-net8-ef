using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Presentation.Controllers;

[ApiController]
[SwaggerTag("Represents journal API")]
public class JournalController(IExceptionJournalService exceptionJournalService) : ControllerBase
{
    /// <summary>
    ///     Retrieves a paginated and optionally filtered list of exception journal entries.
    /// </summary>
    [HttpPost("api.user.journal.getRange")]
    [SwaggerOperation(Description =
        "Provides the pagination API. Skip means the number of items should be skipped by server. " +
        "Take means the maximum number items should be returned by server. All fields of the filter are optional.")]
    public async Task<IActionResult> GetRange([FromBody] GetExceptionJournalRequestRangeDto getExceptionJournalRequestRangeDto)
    {
        var entries = await exceptionJournalService.GetRangeAsync(getExceptionJournalRequestRangeDto);

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
    [HttpPost("api.user.journal.getSingle")]
    [SwaggerOperation(Description = "Returns the information about a particular event by ID.")]
    public async Task<IActionResult> GetSingle([FromBody] GetSingleExceptionJournalRequestDto getSingleExceptionJournalRequestDto)
    {
        var entry = await exceptionJournalService.GetSingleAsync(getSingleExceptionJournalRequestDto);
        return entry is null ? NotFound() : Ok(entry);
    }
}