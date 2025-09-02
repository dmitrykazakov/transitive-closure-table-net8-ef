using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

public interface IServiceFactory
{
    INodeService CreateNodeService();

    ITreeService CreateTreeService();

    IExceptionJournalService CreateExceptionJournalService();
}