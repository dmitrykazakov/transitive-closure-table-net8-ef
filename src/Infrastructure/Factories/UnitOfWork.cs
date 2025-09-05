using Microsoft.EntityFrameworkCore.Storage;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Data.Repositories;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories;

/// <summary>
/// Implements the Unit of Work pattern for managing repositories and database transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="appDbContext">The EF Core database context.</param>
    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        Nodes = new NodeRepository(_appDbContext);
        Trees = new TreeRepository(_appDbContext);
        TransitiveClosures = new TransitiveClosureRepository(_appDbContext);
        ExceptionJournals = new ExceptionJournalRepository(_appDbContext);
    }

    /// <summary>
    /// Gets the node repository.
    /// </summary>
    public INodeRepository Nodes { get; }

    /// <summary>
    /// Gets the tree repository.
    /// </summary>
    public ITreeRepository Trees { get; }

    /// <summary>
    /// Gets the transitive closure repository.
    /// </summary>
    public ITransitiveClosureRepository TransitiveClosures { get; }

    /// <summary>
    /// Gets the exception journal repository.
    /// </summary>
    public IExceptionJournalRepository ExceptionJournals { get; }

    /// <summary>
    /// Commits all pending changes to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> CommitAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <returns>The database transaction instance.</returns>
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _appDbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Disposes the underlying database context and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        _appDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
