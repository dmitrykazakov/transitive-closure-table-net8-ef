using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

/// <summary>
/// Service interface for working with the <see cref="ExceptionJournal"/> table.
/// Provides functionality for logging exceptions and retrieving journal entries.
/// </summary>
public interface IExceptionJournalService
{
    /// <summary>
    /// Logs an exception along with HTTP context information into the database.
    /// </summary>
    /// <param name="exception">The exception instance to log.</param>
    /// <param name="httpContext">The current HTTP context, used to capture query/body parameters.</param>
    /// <returns>
    /// The created <see cref="ExceptionJournal"/> entity that was stored in the database.
    /// </returns>
    Task<ExceptionJournal> LogExceptionAsync(Exception exception, HttpContext httpContext);

    /// <summary>
    /// Retrieves a filtered and paginated list of exception journal entries.
    /// </summary>
    /// <param name="request">The request DTO containing pagination and optional filter parameters.</param>
    /// <returns>
    /// A collection of <see cref="ExceptionJournal"/> entries that match the specified filters.
    /// </returns>
    Task<List<ExceptionJournal>> GetRangeAsync(ExceptionJournalRequestDto request);

    /// <summary>
    /// Retrieves a single exception journal entry by its unique identifier.
    /// </summary>
    /// <param name="eventId">The unique identifier of the exception event.</param>
    /// <returns>
    /// The matching <see cref="ExceptionJournal"/> entry, or <c>null</c> if no entry is found.
    /// </returns>
    Task<ExceptionJournal?> GetSingleAsync(long eventId);
}