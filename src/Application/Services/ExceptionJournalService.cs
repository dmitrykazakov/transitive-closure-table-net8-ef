using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

/// <summary>
///     Service responsible for managing exception logs in the <see cref="ExceptionJournal" /> table.
///     Provides methods for logging exceptions, retrieving single entries, and retrieving paginated/filterable lists.
/// </summary>
public class ExceptionJournalService(IUnitOfWorkFactory unitOfWorkFactory) : IExceptionJournalService
{
    /// <summary>
    ///     Logs an exception along with HTTP context information into the database.
    /// </summary>
    /// <param name="exception">The exception object to log.</param>
    /// <param name="httpContext">The current HTTP context containing request data.</param>
    /// <returns>The created <see cref="ExceptionJournal" /> entity representing the logged exception.</returns>
    public async Task<ExceptionJournal> LogExceptionAsync(Exception exception, HttpContext httpContext)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        var entry = new ExceptionJournal
        {
            Timestamp = DateTime.UtcNow,
            QueryParams = httpContext.Request.QueryString.HasValue
                ? httpContext.Request.QueryString.Value
                : string.Empty,
            BodyParams = httpContext.Items["RequestBody"] as string ?? string.Empty,
            StackTrace = exception.StackTrace ?? string.Empty,
            ExceptionType = exception.GetType().Name
        };

        // Add the exception log entry to the database
        await unitOfWork.ExceptionJournals.AddAsync(entry);

        // Commit transaction to persist the log
        await unitOfWork.CommitAsync();

        return entry;
    }

    /// <summary>
    ///     Retrieves a list of exception journal entries with optional filtering and pagination.
    /// </summary>
    /// <param name="request">
    ///     The request DTO containing pagination parameters (Skip, Take)
    ///     and optional filters (FromTimestamp, ToTimestamp, ExceptionType, QueryContains, BodyContains).
    /// </param>
    /// <returns>A list of <see cref="ExceptionJournal" /> entries matching the filters.</returns>
    public async Task<List<ExceptionJournal>> GetRangeAsync(ExceptionJournalRequestDto request)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        return await unitOfWork.ExceptionJournals.GetRangeAsync(request);
    }

    /// <summary>
    ///     Retrieves a single exception journal entry by its unique identifier.
    /// </summary>
    /// <param name="eventId">The unique ID of the exception event.</param>
    /// <returns>
    ///     The <see cref="ExceptionJournal" /> entry if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<ExceptionJournal?> GetSingleAsync(long eventId)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        return await unitOfWork.ExceptionJournals.GetByIdAsync(eventId);
    }
}