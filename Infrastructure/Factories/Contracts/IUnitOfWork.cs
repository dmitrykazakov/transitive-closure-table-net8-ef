using Microsoft.EntityFrameworkCore.Storage;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

public interface IUnitOfWork : IDisposable
{
    INodeRepository Nodes { get; }
    ITreeRepository Trees { get; }
    ITransitiveClosureRepository TransitiveClosures { get; }
    IExceptionJournalRepository ExceptionJournals { get; }
    Task<int> CommitAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}