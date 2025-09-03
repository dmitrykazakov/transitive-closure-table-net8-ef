using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for logging <see cref="ExceptionJournal"/> entries to the database.
/// </summary>
public class ExceptionJournalRepository(AppDbContext appDbContext) : IExceptionJournalRepository
{
    /// <summary>
    /// Adds a new exception entry to the database and commits the changes.
    /// </summary>
    /// <param name="entry">The <see cref="ExceptionJournal"/> entry to add.</param>
    public async Task AddAsync(ExceptionJournal entry)
    {
        appDbContext.ExceptionJournals.Add(entry);
        await appDbContext.SaveChangesAsync();
    }
}