using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories;

/// <summary>
///     Application factory implementation for Postgres, providing service factories.
/// </summary>
public class PostgresAppFactory(IUnitOfWorkFactory unitOfWorkFactory) : IAppFactory
{
    /// <summary>
    ///     Creates a service factory that provides application service instances.
    /// </summary>
    /// <returns>A new <see cref="IServiceFactory" /> instance.</returns>
    public IServiceFactory CreateServiceFactory()
    {
        return new ServiceFactory(unitOfWorkFactory);
    }
}