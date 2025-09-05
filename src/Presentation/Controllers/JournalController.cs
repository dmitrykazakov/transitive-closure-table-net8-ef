using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Presentation.Controllers;

/// <summary>
/// API controller for managing exception journals.
/// Supports retrieving multiple entries with pagination and filtering, or a single entry by ID.
/// </summary>
[ApiController]
[SwaggerTag("Represents journal API")]
public class JournalController(IExceptionJournalService exceptionJournalService) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated and optionally filtered list of exception journal entries.
    /// </summary>
    /// <remarks>
    /// The request DTO supports optional filters.
    /// - <c>Skip</c> indicates the number of items to skip.
    /// - <c>Take</c> indicates the maximum number of items to return.
    /// </remarks>
    /// <param name="getExceptionJournalRequestRangeDto">DTO containing pagination and filter parameters.</param>
    /// <returns>
    /// Returns <see cref="OkObjectResult"/> containing an object with:
    /// - <c>TotalCount</c>: total number of entries in the query (or returned page)
    /// - <c>Items</c>: list of exception journal entries
    /// </returns>
    [HttpPost("api.user.journal.getRange")]
    [SwaggerOperation(Description =
        "Provides the pagination API. Skip means the number of items should be skipped by server. " +
        "Take means the maximum number of items should be returned by server. All fields of the filter are optional.")]
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
    /// Retrieves a single exception journal entry by its identifier.
    /// </summary>
    /// <param name="getSingleExceptionJournalRequestDto">DTO containing the ID of the journal entry to retrieve.</param>
    /// <returns>
    /// Returns <see cref="OkObjectResult"/> with the entry if found, or <see cref="NotFoundResult"/> if no entry exists for the given ID.
    /// </returns>
    [HttpPost("api.user.journal.getSingle")]
    [SwaggerOperation(Description = "Returns the information about a particular event by ID.")]
    public async Task<IActionResult> GetSingle([FromBody] GetSingleExceptionJournalRequestDto getSingleExceptionJournalRequestDto)
    {
        var entry = await exceptionJournalService.GetSingleAsync(getSingleExceptionJournalRequestDto);
        return entry is null ? NotFound() : Ok(entry);
    }
}
