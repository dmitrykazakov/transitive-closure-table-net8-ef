using TransitiveClosureTable.Application.Services;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories;

public class ServiceFactory(IUnitOfWorkFactory unitOfWorkFactory) : IServiceFactory
{
    public INodeService CreateNodeService()
    {
        return new NodeService(unitOfWorkFactory);
    }

    public ITreeService CreateTreeService()
    {
        return new TreeService(unitOfWorkFactory);
    }

    public IExceptionJournalService CreateExceptionJournalService()
    {
        return new ExceptionJournalService(unitOfWorkFactory);
    }
}