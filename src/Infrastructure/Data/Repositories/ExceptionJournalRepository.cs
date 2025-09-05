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
    public async Task AddAsync(ExceptionJournal entry)
    {
        appDbContext.ExceptionJournals.Add(entry);
        await appDbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves a filtered and paginated list of exception journal entries.
    ///     All filtering and pagination is applied in the repository.
    /// </summary>
    /// <param name="request">
    ///     The request DTO containing optional filters and pagination parameters:
    ///     <see cref="ExceptionJournalRequestDto.Skip" />,
    ///     <see cref="ExceptionJournalRequestDto.Take" />,
    ///     <see cref="ExceptionJournalRequestDto.FromTimestamp" />,
    ///     <see cref="ExceptionJournalRequestDto.ToTimestamp" />,
    ///     <see cref="ExceptionJournalRequestDto.ExceptionType" />,
    ///     <see cref="ExceptionJournalRequestDto.QueryContains" />,
    ///     <see cref="ExceptionJournalRequestDto.BodyContains" />.
    /// </param>
    /// <returns>A list of <see cref="ExceptionJournal" /> entries matching the filters.</returns>
    public async Task<List<ExceptionJournal>> GetRangeAsync(ExceptionJournalRequestDto request)
    {
        var query = appDbContext.ExceptionJournals.AsQueryable();

        if (request.FromTimestamp.HasValue)
            query = query.Where(j => j.Timestamp >= request.FromTimestamp.Value);

        if (request.ToTimestamp.HasValue)
            query = query.Where(j => j.Timestamp <= request.ToTimestamp.Value);

        if (!string.IsNullOrWhiteSpace(request.ExceptionType))
            query = query.Where(j => j.ExceptionType == request.ExceptionType);

        if (!string.IsNullOrWhiteSpace(request.QueryContains))
            query = query.Where(j => j.QueryParams.Contains(request.QueryContains));

        if (!string.IsNullOrWhiteSpace(request.BodyContains))
            query = query.Where(j => j.BodyParams.Contains(request.BodyContains));

        return await query
            .OrderByDescending(j => j.Timestamp)
            .Skip(request.Skip)
            .Take(request.Take)
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