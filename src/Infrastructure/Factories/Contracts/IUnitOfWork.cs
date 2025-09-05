using Microsoft.EntityFrameworkCore.Storage;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

/// <summary>
///     Represents a unit of work, encapsulating repositories and transaction management.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    ///     Repository for managing <see cref="TransitiveClosureTable.Domain.Entities.Node" /> entities.
    /// </summary>
    INodeRepository Nodes { get; }

    /// <summary>
    ///     Repository for managing <see cref="TransitiveClosureTable.Domain.Entities.Tree" /> entities.
    /// </summary>
    ITreeRepository Trees { get; }

    /// <summary>
    ///     Repository for managing <see cref="TransitiveClosureTable.Domain.Entities.TransitiveClosure" /> entities.
    /// </summary>
    ITransitiveClosureRepository TransitiveClosures { get; }

    /// <summary>
    ///     Repository for logging exceptions in <see cref="TransitiveClosureTable.Domain.Entities.ExceptionJournal" />.
    /// </summary>
    IExceptionJournalRepository ExceptionJournals { get; }

    /// <summary>
    ///     Saves all changes made in this unit of work.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> CommitAsync();

    /// <summary>
    ///     Begins a new database transaction.
    /// </summary>
    /// <returns>An <see cref="IDbContextTransaction" /> representing the started transaction.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();
}