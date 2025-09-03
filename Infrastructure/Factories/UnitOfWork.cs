using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Data.Repositories;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        Nodes = new NodeRepository(_appDbContext);
        Trees = new TreeRepository(_appDbContext);
        TransitiveClosures = new TransitiveClosureRepository(_appDbContext);
        ExceptionJournals = new ExceptionJournalRepository(_appDbContext);
    }

    public INodeRepository Nodes { get; }
    public ITreeRepository Trees { get; }
    public ITransitiveClosureRepository TransitiveClosures { get; }
    public IExceptionJournalRepository ExceptionJournals { get; }

    public async Task<int> CommitAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _appDbContext.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _appDbContext.Dispose();

        // Prevent the finalizer from running.
        GC.SuppressFinalize(this);
    }
}