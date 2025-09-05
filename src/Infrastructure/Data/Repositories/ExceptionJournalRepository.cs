using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

/// <summary>
///     Repository implementation for logging <see cref="ExceptionJournal" /> entries to the database.
///     Handles all filtering, pagination, and retrieval logic for exception logs.
/// </summary>
public class ExceptionJournalRepository(AppDbContext appDbContext) : IExceptionJournalRepository
{
    /// <summary>
    ///     Adds a new exception entry to the database and commits the changes.
    /// </summary>
    /// <param name="entry">The <see cref="ExceptionJournal" /> entry to add.</param>
    public Task AddAsync(ExceptionJournal entry)
    {
        appDbContext.ExceptionJournals.Add(entry);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieves a filtered and paginated list of exception journal entries.
    ///     All filtering and pagination is applied in the repository.
    /// </summary>
    /// <param name="requestRange">
    ///     The requestRange DTO containing optional filters and pagination parameters:
    ///     <see cref="GetExceptionJournalRequestRangeDto.Skip" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.Take" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.FromTimestamp" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.ToTimestamp" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.ExceptionType" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.QueryContains" />,
    ///     <see cref="GetExceptionJournalRequestRangeDto.BodyContains" />.
    /// </param>
    /// <returns>A list of <see cref="ExceptionJournal" /> entries matching the filters.</returns>
    public async Task<List<ExceptionJournal>> GetRangeAsync(GetExceptionJournalRequestRangeDto requestRange)
    {
        var query = appDbContext.ExceptionJournals.AsQueryable();

        if (requestRange.FromTimestamp.HasValue)
            query = query.Where(j => j.Timestamp >= requestRange.FromTimestamp.Value);

        if (requestRange.ToTimestamp.HasValue)
            query = query.Where(j => j.Timestamp <= requestRange.ToTimestamp.Value);

        if (!string.IsNullOrWhiteSpace(requestRange.ExceptionType))
            query = query.Where(j => j.ExceptionType == requestRange.ExceptionType);

        if (!string.IsNullOrWhiteSpace(requestRange.QueryContains))
            query = query.Where(j => j.QueryParams.Contains(requestRange.QueryContains));

        if (!string.IsNullOrWhiteSpace(requestRange.BodyContains))
            query = query.Where(j => j.BodyParams.Contains(requestRange.BodyContains));

        return await query
            .OrderByDescending(j => j.Timestamp)
            .Skip(requestRange.Skip)
            .Take(requestRange.Take)
            .ToListAsync();
    }

    /// <summary>
    ///     Retrieves a single exception journal entry by its unique identifier.
    /// </summary>
    /// <param name="eventId">The unique ID of the exception event.</param>
    /// <returns>The <see cref="ExceptionJournal" /> entry if found; otherwise <c>null</c>.</returns>
    public async Task<ExceptionJournal?> GetByIdAsync(long eventId)
    {
        return await appDbContext.ExceptionJournals
            .FirstOrDefaultAsync(j => j.EventId == eventId);
    }
}