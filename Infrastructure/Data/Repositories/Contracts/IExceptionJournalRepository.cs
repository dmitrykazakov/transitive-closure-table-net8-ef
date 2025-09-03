using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

/// <summary>
/// Repository interface for logging exceptions to the database.
/// </summary>
public interface IExceptionJournalRepository
{
    /// <summary>
    /// Adds a new <see cref="ExceptionJournal"/> entry to the database.
    /// </summary>
    /// <param name="entry">The exception entry to log.</param>
    Task AddAsync(ExceptionJournal entry);
}