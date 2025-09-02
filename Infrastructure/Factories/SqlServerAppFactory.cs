using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories;

public class SqlServerAppFactory(IUnitOfWorkFactory unitOfWorkFactory) : IAppFactory
{
    public IServiceFactory CreateServiceFactory()
    {
        return new ServiceFactory(unitOfWorkFactory);
    }
}