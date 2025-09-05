using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

/// <summary>
///     Repository interface for performing CRUD operations on <see cref="ExceptionJournal" /> entries.
///     Handles all database-level queries and filtering logic.
/// </summary>
public interface IExceptionJournalRepository
{
    /// <summary>
    ///     Adds a new exception log entry to the database.
    /// </summary>
    /// <param name="entry">The <see cref="ExceptionJournal" /> entity to add.</param>
    Task AddAsync(ExceptionJournal entry);

    /// <summary>
    ///     Retrieves a filtered and paginated list of exception journal entries.
    ///     All filtering, ordering, and pagination logic is implemented in the repository.
    /// </summary>
    /// <param name="requestRange">
    ///     The requestRange DTO containing optional filters:
    ///     <see cref="GetExceptionJournalRequestRangeDto.Skip" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.Take" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.FromTimestamp" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.ToTimestamp" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.ExceptionType" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.QueryContains" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.BodyContains" />.
    /// </param>
    /// <returns>A list of <see cref="ExceptionJournal" /> entries matching the filters.</returns>
    Task<List<ExceptionJournal>> GetRangeAsync(GetExceptionJournalRequestRangeDto requestRange);

    /// <summary>
    ///     Retrieves a single exception journal entry by its unique identifier.
    /// </summary>
    /// <param name="eventId">The unique ID of the exception event.</param>
    /// <returns>The <see cref="ExceptionJournal" /> entry if found; otherwise <c>null</c>.</returns>
    Task<ExceptionJournal?> GetByIdAsync(long eventId);
}