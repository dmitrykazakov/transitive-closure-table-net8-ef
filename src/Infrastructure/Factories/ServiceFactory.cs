using TransitiveClosureTable.Application.Services;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories;

/// <summary>
///     Factory for creating application service instances, using a unit of work factory.
/// </summary>
public class ServiceFactory(IUnitOfWorkFactory unitOfWorkFactory) : IServiceFactory
{
    /// <summary>
    ///     Creates an instance of <see cref="INodeService" />.
    /// </summary>
    /// <returns>A new <see cref="NodeService" />.</returns>
    public INodeService CreateNodeService()
    {
        return new NodeService(unitOfWorkFactory);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ITreeService" />.
    /// </summary>
    /// <returns>A new <see cref="TreeService" />.</returns>
    public ITreeService CreateTreeService()
    {
        return new TreeService(unitOfWorkFactory);
    }

    /// <summary>
    ///     Creates an instance of <see cref="IExceptionJournalService" />.
    /// </summary>
    /// <returns>A new <see cref="ExceptionJournalService" />.</returns>
    public IExceptionJournalService CreateExceptionJournalService()
    {
        return new ExceptionJournalService(unitOfWorkFactory);
    }
}