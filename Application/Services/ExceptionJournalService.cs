using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

/// <summary>
/// Service for logging exceptions into the ExceptionJournal table.
/// </summary>
public class ExceptionJournalService(IUnitOfWorkFactory unitOfWorkFactory) : IExceptionJournalService
{
    /// <summary>
    /// Logs an exception with httpContext information into the database.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="httpContext">The current HTTP httpContext.</param>
    /// <returns>The created <see cref="ExceptionJournal"/> entity.</returns>
    public async Task<ExceptionJournal> LogExceptionAsync(Exception exception, HttpContext httpContext)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        var exceptionJournal = new ExceptionJournal
        {
            Timestamp = DateTime.UtcNow,
            QueryParams = httpContext.Request.QueryString.HasValue ? httpContext.Request.QueryString.Value : string.Empty,
            BodyParams = httpContext.Items["RequestBody"] as string ?? string.Empty,
            StackTrace = exception.StackTrace ?? string.Empty,
            ExceptionType = exception.GetType().Name
        };

        // Add the exception log entry to the database
        await unitOfWork.ExceptionJournals.AddAsync(exceptionJournal);

        // Commit the transaction to persist the log
        await unitOfWork.CommitAsync();

        return exceptionJournal;
    }
}