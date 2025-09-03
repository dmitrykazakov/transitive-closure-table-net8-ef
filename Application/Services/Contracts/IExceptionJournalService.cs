using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

/// <summary>
/// Service interface for logging exceptions into the ExceptionJournal table.
/// </summary>
public interface IExceptionJournalService
{
    /// <summary>
    /// Logs an exception along with HTTP context information into the database.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <returns>The created <see cref="ExceptionJournal"/> entity.</returns>
    Task<ExceptionJournal> LogExceptionAsync(Exception exception, HttpContext httpContext);
}